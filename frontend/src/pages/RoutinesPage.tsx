import { useState } from 'react'
import { fetchHome } from '../api/home'
import { createRoutine, deleteRoutine, fetchRoutines } from '../api/routines'
import { setCurrentRoutine } from '../api/users'
import { useAuth } from '../auth/context'
import { EmptyState, ErrorList, LoadingState } from '../components/Feedback'
import { InlineNameForm } from '../components/InlineNameForm'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useConfirm } from '../hooks/useConfirm'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'

export function RoutinesPage() {
  const { t } = useI18n()
  const { token } = useAuth()
  const { navigate } = useNavigation()
  const routines = useApiQuery('routines', (token) => fetchRoutines(token))
  const home = useApiQuery('home', (token) => fetchHome(token))
  const { confirm, dialog } = useConfirm()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<unknown>(null)

  const currentRoutineId = home.data?.currentRoutine?.routineId ?? null

  async function run(action: () => Promise<unknown>) {
    setBusy(true)
    setError(null)
    try {
      await action()
      routines.reload()
      home.reload()
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
    }
  }

  async function handleDelete(routineId: string, name: string) {
    const accepted = await confirm({
      title: t('common.delete'),
      body: t('routine.confirmDeleteRoutine', { name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) {
      await run(() => deleteRoutine(token, routineId))
    }
  }

  return (
    <>
      <ScreenHeader title={t('menu.routines')} />

      <InlineNameForm
        placeholder={t('routines.namePlaceholder')}
        submitLabel={t('routines.new')}
        disabled={busy}
        onSubmit={(name) => void run(() => createRoutine(token, name))}
      />

      {error !== null && <ErrorList error={error} />}
      {routines.isLoading && <LoadingState />}
      {routines.error !== null && <ErrorList error={routines.error} />}

      {!routines.isLoading && routines.error === null && routines.data?.length === 0 && (
        <EmptyState title={t('routines.empty')} body={t('routines.emptyBody')} />
      )}

      <div className="card-list">
        {routines.data?.map((routine) => (
          <article key={routine.id} className="card list-row">
            <div className="list-row-main">
              <h2 className="list-row-title">
                {routine.name}
                {routine.id === currentRoutineId && (
                  <span className="badge">{t('routines.current')}</span>
                )}
              </h2>
            </div>
            <div className="button-row">
              <button
                className="btn btn-ghost"
                type="button"
                disabled={busy || routine.id === currentRoutineId}
                onClick={() => void run(() => setCurrentRoutine(token, routine.id))}
              >
                {t('routines.makeCurrent')}
              </button>
              <button
                className="btn btn-primary"
                type="button"
                onClick={() => navigate({ name: 'routine', routineId: routine.id })}
              >
                {t('routines.open')}
              </button>
              <button
                className="btn btn-danger"
                type="button"
                disabled={busy}
                onClick={() => void handleDelete(routine.id, routine.name)}
              >
                {t('common.delete')}
              </button>
            </div>
          </article>
        ))}
      </div>
      {dialog}
    </>
  )
}
