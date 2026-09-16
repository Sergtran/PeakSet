import { useState } from 'react'
import { History, LineChart, Plus, Trash2, X } from 'lucide-react'
import { fetchRoutine, type Session } from '@/api/routines'
import type { ExerciseType, Laterality } from '@/api/types'
import { createWorkout, type WorkoutRequest } from '@/api/workouts'
import { useAuth } from '@/auth/context'
import {
  ExerciseInsightSheet,
  type InsightView,
} from '@/components/ExerciseInsightSheet'
import { FloatingTimer } from '@/components/FloatingTimer'
import { PageHeader } from '@/components/PageHeader'
import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Skeleton } from '@/components/ui/skeleton'
import { useApiQuery } from '@/hooks/useApiQuery'
import { describeApiError } from '@/i18n/apiErrors'
import { useI18n } from '@/i18n/context'
import { useNavigation } from '@/navigation/context'
import { useSettings } from '@/settings/context'
import { toKilograms } from '@/utils/units'

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

const firstSetCount = 1

type SessionPageProps = {
  routineId: string
  sessionId: string
  sessionName: string
}

export function SessionPage({ routineId, sessionId, sessionName }: SessionPageProps) {
  const { t } = useI18n()
  const detail = useApiQuery(`routine:${routineId}`, (token) => fetchRoutine(token, routineId))
  const session = detail.data?.sessions.find((entry) => entry.id === sessionId) ?? null

  if (detail.error !== null) {
    return (
      <PageHeader title={sessionName}>
        {describeApiError(detail.error, t).map((message) => (
          <p
            key={message}
            className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive"
          >
            {message}
          </p>
        ))}
      </PageHeader>
    )
  }

  if (!session) {
    return (
      <PageHeader title={sessionName}>
        <Skeleton className="h-40 w-full rounded-xl" />
      </PageHeader>
    )
  }

  return (
    <SessionRunner
      key={session.id}
      routineId={routineId}
      routineName={detail.data?.name ?? ''}
      session={session}
    />
  )
}

function SessionRunner({
  routineId,
  routineName,
  session,
}: {
  routineId: string
  routineName: string
  session: Session
}) {
  const { t } = useI18n()
  const { token } = useAuth()
  const { navigate } = useNavigation()
  const { settings } = useSettings()
  const [draft, setDraft] = useState<DraftExercise[]>(() =>
    session.exercises.map((exercise) => ({
      name: exercise.name,
      exerciseType: exercise.exerciseType,
      laterality: exercise.laterality,
      sets: Array.from({ length: firstSetCount }, () => ({ reps: '', weight: '' })),
    })),
  )
  const [insight, setInsight] = useState<{ name: string; view: InsightView } | null>(null)
  const [saving, setSaving] = useState(false)
  const [failure, setFailure] = useState<unknown | null>(null)

  function changeSet(exerciseIndex: number, setIndex: number, patch: Partial<DraftSet>) {
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

  const filled = draft
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
    setFailure(null)
    try {
      const payload: WorkoutRequest = {
        routineId,
        routineName,
        sessionName: session.name,
        workoutDate: new Date().toISOString(),
        exercises: filled,
      }
      await createWorkout(token, payload)
      navigate({ name: 'history' })
    } catch (cause) {
      setFailure(cause)
    } finally {
      setSaving(false)
    }
  }

  return (
    <>
      <PageHeader
        title={session.name}
        subtitle={routineName}
        onBack={() => navigate({ name: 'train' })}
      />

      <div className="grid gap-4 pb-28">
        {failure !== null &&
          describeApiError(failure, t).map((message) => (
            <p
              key={message}
              className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive"
            >
              {message}
            </p>
          ))}

        {draft.length === 0 && (
          <p className="text-sm text-muted-foreground">{t('train.noExercises')}</p>
        )}

        {draft.map((exercise, exerciseIndex) => (
          <Card key={exercise.name}>
            <CardContent className="grid gap-4 pt-6">
              <div className="grid gap-1">
                <h2 className="font-semibold leading-tight">{exercise.name}</h2>
                <p className="text-xs text-muted-foreground">
                  {t(`exerciseType.${exercise.exerciseType}`)} ·{' '}
                  {t(`laterality.${exercise.laterality}`)}
                </p>
              </div>

              <div className="flex gap-2">
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => setInsight({ name: exercise.name, view: 'history' })}
                >
                  <History className="size-4" />
                  {t('menu.history')}
                </Button>
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => setInsight({ name: exercise.name, view: 'chart' })}
                >
                  <LineChart className="size-4" />
                  {t('menu.stats')}
                </Button>
              </div>

              <div className="grid grid-cols-[2rem_1fr_1fr_2rem] items-center gap-2">
                <span className="text-[0.7rem] font-semibold uppercase text-muted-foreground">
                  {t('train.set')}
                </span>
                <span className="text-[0.7rem] font-semibold uppercase text-muted-foreground">
                  {t('train.reps')}
                </span>
                <span className="text-[0.7rem] font-semibold uppercase text-muted-foreground">
                  {t('train.weight')} ({settings.unit})
                </span>
                <span />

                {exercise.sets.map((set, setIndex) => (
                  <SetRow
                    key={setIndex}
                    index={setIndex}
                    set={set}
                    onChange={(patch) => changeSet(exerciseIndex, setIndex, patch)}
                    onRemove={() => removeSet(exerciseIndex, setIndex)}
                  />
                ))}
              </div>

              <Button
                variant="ghost"
                size="sm"
                className="w-fit"
                onClick={() => addSet(exerciseIndex)}
              >
                <Plus className="size-4" />
                {t('train.addSet')}
              </Button>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="pointer-events-none fixed inset-x-0 bottom-28 z-20 px-4 md:bottom-20">
        <Button
          size="lg"
          className="pointer-events-auto mx-auto flex h-12 w-full max-w-md"
          disabled={saving || filled.length === 0}
          onClick={() => void handleSave()}
        >
          {saving ? t('train.saving') : t('train.save')}
        </Button>
      </div>

      <FloatingTimer />
      <ExerciseInsightSheet exercise={insight} onClose={() => setInsight(null)} />
    </>
  )
}

function SetRow({
  index,
  set,
  onChange,
  onRemove,
}: {
  index: number
  set: DraftSet
  onChange: (patch: Partial<DraftSet>) => void
  onRemove: () => void
}) {
  return (
    <>
      <span className="text-center text-sm font-semibold tabular-nums text-muted-foreground">
        {index + 1}
      </span>
      <Input
        type="number"
        inputMode="numeric"
        min={0}
        className="h-11 text-center"
        value={set.reps}
        onChange={(event) => onChange({ reps: event.target.value })}
      />
      <Input
        type="number"
        inputMode="decimal"
        min={0}
        step="0.5"
        className="h-11 text-center"
        value={set.weight}
        onChange={(event) => onChange({ weight: event.target.value })}
      />
      <Button
        size="icon"
        variant="ghost"
        className="size-8 text-muted-foreground"
        aria-label="remove"
        onClick={onRemove}
      >
        {index === 0 ? <X className="size-4" /> : <Trash2 className="size-4" />}
      </Button>
    </>
  )
}
