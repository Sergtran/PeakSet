import { useState } from 'react'
import { deleteWorkout, fetchWorkout } from '../api/workouts'
import { useAuth } from '../auth/context'
import { ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useConfirm } from '../hooks/useConfirm'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'
import { useSettings } from '../settings/context'
import { formatDateTime } from '../utils/format'
import { fromKilograms, withUnit } from '../utils/units'

export function WorkoutPage({ workoutId }: { workoutId: string }) {
  const { t, language } = useI18n()
  const { token } = useAuth()
  const { back } = useNavigation()
  const { settings } = useSettings()
  const query = useApiQuery(`workout:${workoutId}`, (token) => fetchWorkout(token, workoutId))
  const { confirm, dialog } = useConfirm()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<unknown>(null)
  const workout = query.data

  async function handleDelete() {
    const accepted = await confirm({
      title: t('common.delete'),
      body: t('workout.confirmDelete'),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (!accepted) return
    setBusy(true)
    setError(null)
    try {
      await deleteWorkout(token, workoutId)
      back()
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
    }
  }

  if (query.error !== null) {
    return (
      <>
        <ScreenHeader title={t('menu.history')} />
        <ErrorList error={query.error} />
      </>
    )
  }

  if (!workout) {
    return (
      <>
        <ScreenHeader title={t('menu.history')} />
        <LoadingState />
      </>
    )
  }

  return (
    <>
      <ScreenHeader
        title={workout.sessionName}
        subtitle={`${workout.routineName} · ${formatDateTime(workout.workoutDate, language)}`}
      />

      {error !== null && <ErrorList error={error} />}

      <div className="card-list">
        {workout.exercises.map((exercise) => (
          <article key={exercise.id} className="card session-card">
            <h2 className="list-row-title">
              {exercise.name}
              {exercise.prStatus === 'New' && <span className="badge badge-pr">{t('workout.pr')}</span>}
              {exercise.prStatus === 'Matched' && (
                <span className="badge">{t('workout.prMatched')}</span>
              )}
            </h2>
            <div className="set-table">
              <span className="set-head">{t('train.set')}</span>
              <span className="set-head">{t('train.reps')}</span>
              <span className="set-head">{t('train.weight')}</span>
              <span />
              {exercise.sets.map((set) => (
                <SetLine key={set.setNumber} set={set} unit={settings.unit} />
              ))}
            </div>
          </article>
        ))}
      </div>

      <div className="button-row">
        <button className="btn btn-danger" type="button" disabled={busy} onClick={() => void handleDelete()}>
          {t('common.delete')}
        </button>
      </div>
      {dialog}
    </>
  )
}

function SetLine({
  set,
  unit,
}: {
  set: { setNumber: number; reps: number; weight: number }
  unit: 'kg' | 'lb'
}) {
  return (
    <>
      <span className="set-number">{set.setNumber}</span>
      <span className="set-value">{set.reps}</span>
      <span className="set-value">{withUnit(fromKilograms(set.weight, unit), unit)}</span>
      <span />
    </>
  )
}
