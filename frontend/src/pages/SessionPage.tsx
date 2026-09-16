import { useState } from 'react'
import { fetchRoutine, type Session } from '../api/routines'
import type { ExerciseType, Laterality } from '../api/types'
import {
  createWorkout,
  fetchWorkouts,
  type PagedResult,
  type Workout,
  type WorkoutRequest,
} from '../api/workouts'
import { useAuth } from '../auth/context'
import { ErrorList, LoadingState } from '../components/Feedback'
import { IntervalTimer } from '../components/IntervalTimer'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'
import { useSettings } from '../settings/context'
import type { WeightUnit } from '../settings/types'
import { fromKilograms, toKilograms } from '../utils/units'

type DraftSet = {
  reps: string
  weight: string
}

type DraftExercise = {
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
  sets: DraftSet[]
}

type PreviousSet = {
  reps: number
  weight: number
}

const defaultSetCount = 3
const recentWorkoutCount = 20

type SessionPageProps = {
  routineId: string
  sessionId: string
  sessionName: string
}

export function SessionPage({ routineId, sessionId, sessionName }: SessionPageProps) {
  const detail = useApiQuery(`routine:${routineId}`, (token) => fetchRoutine(token, routineId))
  const recent = useApiQuery('workouts:recent', (token) =>
    fetchWorkouts(token, 1, recentWorkoutCount),
  )
  const session = detail.data?.sessions.find((entry) => entry.id === sessionId) ?? null
  const error = detail.error ?? recent.error

  if (error !== null) {
    return (
      <>
        <ScreenHeader title={sessionName} />
        <ErrorList error={error} />
      </>
    )
  }

  if (!session || recent.data === null) {
    return (
      <>
        <ScreenHeader title={sessionName} />
        <LoadingState />
      </>
    )
  }

  return (
    <SessionRunner
      key={session.id}
      routineId={routineId}
      routineName={detail.data?.name ?? ''}
      session={session}
      previousSets={buildPreviousSets(recent.data)}
    />
  )
}

/**
 * Walks the most recent workouts (newest first) and keeps the last sets used for
 * each exercise name, so the lifter does not retype last week's numbers.
 */
function buildPreviousSets(result: PagedResult<Workout>): Map<string, PreviousSet[]> {
  const previous = new Map<string, PreviousSet[]>()

  for (const workout of result.items) {
    for (const exercise of workout.exercises) {
      if (!previous.has(exercise.name)) {
        previous.set(
          exercise.name,
          exercise.sets.map((set) => ({ reps: set.reps, weight: set.weight })),
        )
      }
    }
  }

  return previous
}

function buildDraft(
  session: Session,
  previousSets: Map<string, PreviousSet[]>,
  unit: WeightUnit,
): DraftExercise[] {
  return session.exercises.map((exercise) => {
    const previous = previousSets.get(exercise.name)

    return {
      name: exercise.name,
      exerciseType: exercise.exerciseType,
      laterality: exercise.laterality,
      sets:
        previous && previous.length > 0
          ? previous.map((set) => ({
              reps: String(set.reps),
              weight: String(fromKilograms(set.weight, unit)),
            }))
          : Array.from({ length: defaultSetCount }, () => ({ reps: '', weight: '' })),
    }
  })
}

type SessionRunnerProps = {
  routineId: string
  routineName: string
  session: Session
  previousSets: Map<string, PreviousSet[]>
}

function SessionRunner({ routineId, routineName, session, previousSets }: SessionRunnerProps) {
  const { t } = useI18n()
  const { token } = useAuth()
  const { navigate } = useNavigation()
  const { settings } = useSettings()
  const [draft, setDraft] = useState<DraftExercise[]>(() =>
    buildDraft(session, previousSets, settings.unit),
  )
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<unknown>(null)

  function updateSet(exerciseIndex: number, setIndex: number, patch: Partial<DraftSet>) {
    setDraft((current) =>
      current.map((exercise, index) =>
        index === exerciseIndex
          ? {
              ...exercise,
              sets: exercise.sets.map((set, position) =>
                position === setIndex ? { ...set, ...patch } : set,
              ),
            }
          : exercise,
      ),
    )
  }

  function addSet(exerciseIndex: number) {
    setDraft((current) =>
      current.map((exercise, index) =>
        index === exerciseIndex
          ? { ...exercise, sets: [...exercise.sets, { reps: '', weight: '' }] }
          : exercise,
      ),
    )
  }

  function removeSet(exerciseIndex: number, setIndex: number) {
    setDraft((current) =>
      current.map((exercise, index) =>
        index === exerciseIndex
          ? { ...exercise, sets: exercise.sets.filter((_, position) => position !== setIndex) }
          : exercise,
      ),
    )
  }

  const filledExercises = draft
    .map((exercise) => ({
      name: exercise.name,
      exerciseType: exercise.exerciseType,
      laterality: exercise.laterality,
      sets: exercise.sets
        .filter((set) => Number(set.reps) > 0)
        .map((set) => ({
          reps: Number(set.reps),
          weight: toKilograms(Number(set.weight) || 0, settings.unit),
        })),
    }))
    .filter((exercise) => exercise.sets.length > 0)

  async function handleSave() {
    setSaving(true)
    setError(null)
    try {
      const payload: WorkoutRequest = {
        routineId,
        routineName,
        sessionName: session.name,
        workoutDate: new Date().toISOString(),
        exercises: filledExercises,
      }
      await createWorkout(token, payload)
      navigate({ name: 'history' })
    } catch (cause) {
      setError(cause)
    } finally {
      setSaving(false)
    }
  }

  return (
    <>
      <ScreenHeader title={session.name} subtitle={routineName} />

      <IntervalTimer config={settings} />

      {error !== null && <ErrorList error={error} />}

      {draft.length === 0 && <p className="list-row-meta">{t('train.noExercises')}</p>}

      <div className="card-list">
        {draft.map((exercise, exerciseIndex) => (
          <article key={exercise.name} className="card session-card">
            <h2 className="list-row-title">{exercise.name}</h2>
            <p className="list-row-meta">
              {t(`exerciseType.${exercise.exerciseType}`)} · {t(`laterality.${exercise.laterality}`)}
            </p>

            <div className="set-table">
              <span className="set-head">{t('train.set')}</span>
              <span className="set-head">{t('train.reps')}</span>
              <span className="set-head">
                {t('train.weight')} ({settings.unit})
              </span>
              <span />
              {exercise.sets.map((set, setIndex) => (
                <SetRow
                  key={setIndex}
                  index={setIndex}
                  set={set}
                  onChange={(patch) => updateSet(exerciseIndex, setIndex, patch)}
                  onRemove={() => removeSet(exerciseIndex, setIndex)}
                />
              ))}
            </div>

            <button className="btn btn-ghost" type="button" onClick={() => addSet(exerciseIndex)}>
              {t('train.addSet')}
            </button>
          </article>
        ))}
      </div>

      {draft.length > 0 && (
        <div className="button-row sticky-actions">
          <button
            className="btn btn-primary"
            type="button"
            disabled={saving || filledExercises.length === 0}
            onClick={() => void handleSave()}
          >
            {saving ? t('train.saving') : t('train.save')}
          </button>
        </div>
      )}
    </>
  )
}

type SetRowProps = {
  index: number
  set: DraftSet
  onChange: (patch: Partial<DraftSet>) => void
  onRemove: () => void
}

function SetRow({ index, set, onChange, onRemove }: SetRowProps) {
  return (
    <>
      <span className="set-number">{index + 1}</span>
      <input
        className="field-input"
        type="number"
        inputMode="numeric"
        min={0}
        value={set.reps}
        onChange={(event) => onChange({ reps: event.target.value })}
      />
      <input
        className="field-input"
        type="number"
        inputMode="decimal"
        min={0}
        step="0.5"
        value={set.weight}
        onChange={(event) => onChange({ weight: event.target.value })}
      />
      <button className="btn btn-ghost" type="button" onClick={onRemove}>
        ✕
      </button>
    </>
  )
}
