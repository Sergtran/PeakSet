import { useState } from 'react'
import { fetchExercises } from '../api/exercises'
import { EmptyState, ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'
import { formatDay } from '../utils/format'

export function StatsPage() {
  const { t, language } = useI18n()
  const { navigate } = useNavigation()
  const query = useApiQuery('exercises', (token) => fetchExercises(token))
  const [filter, setFilter] = useState('')

  const needle = filter.trim().toLowerCase()
  const items = (query.data ?? []).filter((entry) => entry.name.toLowerCase().includes(needle))

  return (
    <>
      <ScreenHeader title={t('menu.stats')} />

      {query.isLoading && <LoadingState />}
      {query.error !== null && <ErrorList error={query.error} />}

      {query.data?.length === 0 && (
        <EmptyState title={t('stats.noExercises')} body={t('stats.noExercisesBody')} />
      )}

      {(query.data?.length ?? 0) > 0 && (
        <>
          <input
            className="field-input"
            placeholder={t('stats.search')}
            value={filter}
            onChange={(event) => setFilter(event.target.value)}
          />

          <div className="card-list">
            {items.map((exercise) => (
              <button
                key={exercise.name}
                type="button"
                className="card list-row list-row-button"
                onClick={() => navigate({ name: 'exercise', exerciseName: exercise.name })}
              >
                <div className="list-row-main">
                  <h2 className="list-row-title">{exercise.name}</h2>
                  <p className="list-row-meta">
                    {t('stats.sessions')}: {exercise.sessionCount}
                    {exercise.lastUsed
                      ? ` · ${t('stats.lastSeen')}: ${formatDay(exercise.lastUsed, language)}`
                      : ''}
                  </p>
                </div>
              </button>
            ))}
          </div>
        </>
      )}
    </>
  )
}
