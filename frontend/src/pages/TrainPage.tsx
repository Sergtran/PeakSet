import { fetchHome } from '../api/home'
import { fetchRoutine } from '../api/routines'
import { EmptyState, ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'

export function TrainPage() {
  const { t } = useI18n()
  const { navigate } = useNavigation()
  const home = useApiQuery('home', (token) => fetchHome(token))
  const routineId = home.data?.currentRoutine?.routineId ?? null
  const detail = useApiQuery(
    `routine:${routineId ?? 'none'}`,
    (token) => (routineId ? fetchRoutine(token, routineId) : Promise.resolve(null)),
  )
  const routine = detail.data

  return (
    <>
      <ScreenHeader title={t('train.title')} subtitle={routineId ? t('train.pickSession') : undefined} />

      {home.isLoading && <LoadingState />}
      {home.error !== null && <ErrorList error={home.error} />}

      {!home.isLoading && home.error === null && !routineId && (
        <EmptyState title={t('train.noRoutine')} body={t('train.noRoutineBody')}>
          <button className="btn btn-primary" type="button" onClick={() => navigate({ name: 'routines' })}>
            {t('menu.routines')}
          </button>
        </EmptyState>
      )}

      {detail.error !== null && <ErrorList error={detail.error} />}
      {routineId !== null && detail.error === null && routine === null && <LoadingState />}

      {routine && (
        <div className="card-list">
          {routine.sessions.length === 0 && (
            <EmptyState title={t('routine.noSessions')} body={t('routine.noSessionsBody')} />
          )}
          {routine.sessions.map((session) => (
            <article key={session.id} className="card list-row">
              <div>
                <h2 className="list-row-title">{session.name}</h2>
                <p className="list-row-meta">
                  {session.exercises.length === 0
                    ? t('train.noExercises')
                    : session.exercises.map((exercise) => exercise.name).join(' · ')}
                </p>
              </div>
              <button
                className="btn btn-primary"
                type="button"
                onClick={() =>
                  navigate({
                    name: 'session',
                    routineId: routine.id,
                    sessionId: session.id,
                    sessionName: session.name,
                  })
                }
              >
                {t('train.start')}
              </button>
            </article>
          ))}
        </div>
      )}
    </>
  )
}
