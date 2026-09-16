import { useState } from 'react'
import { fetchWorkouts } from '../api/workouts'
import { EmptyState, ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'
import { formatDateTime } from '../utils/format'

const pageSize = 20

export function HistoryPage() {
  const { t, language } = useI18n()
  const { navigate } = useNavigation()
  const [page, setPage] = useState(1)
  const query = useApiQuery(`workouts:${page}`, (token) => fetchWorkouts(token, page, pageSize))
  const result = query.data

  return (
    <>
      <ScreenHeader
        title={t('menu.history')}
        subtitle={result ? t('history.total', { count: result.totalCount }) : undefined}
      />

      {query.isLoading && <LoadingState />}
      {query.error !== null && <ErrorList error={query.error} />}

      {result && result.totalCount === 0 && (
        <EmptyState title={t('history.empty')} body={t('history.emptyBody')} />
      )}

      <div className="card-list">
        {result?.items.map((workout) => (
          <button
            key={workout.id}
            type="button"
            className="card list-row list-row-button"
            onClick={() => navigate({ name: 'workout', workoutId: workout.id })}
          >
            <div className="list-row-main">
              <h2 className="list-row-title">{workout.sessionName}</h2>
              <p className="list-row-meta">
                {workout.routineName} · {formatDateTime(workout.workoutDate, language)}
              </p>
            </div>
            <span className="badge">{workout.exercises.length}</span>
          </button>
        ))}
      </div>

      {result && result.totalPages > 1 && (
        <div className="button-row pagination">
          <button
            className="btn btn-ghost"
            type="button"
            disabled={page <= 1}
            onClick={() => setPage((current) => current - 1)}
          >
            ←
          </button>
          <span className="list-row-meta">
            {t('history.page', { page: result.page, total: result.totalPages })}
          </span>
          <button
            className="btn btn-ghost"
            type="button"
            disabled={page >= result.totalPages}
            onClick={() => setPage((current) => current + 1)}
          >
            →
          </button>
        </div>
      )}
    </>
  )
}
