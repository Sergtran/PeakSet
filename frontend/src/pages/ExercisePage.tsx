import { fetchExerciseProgress } from '../api/exercises'
import { ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useSettings } from '../settings/context'
import { formatDay } from '../utils/format'
import { fromKilograms } from '../utils/units'

export function ExercisePage({ exerciseName }: { exerciseName: string }) {
  const { t, language } = useI18n()
  const { settings } = useSettings()
  const query = useApiQuery(
    `exercise:${exerciseName}`,
    (token) => fetchExerciseProgress(token, exerciseName),
  )
  const progress = query.data

  if (query.error !== null) {
    return (
      <>
        <ScreenHeader title={exerciseName} />
        <ErrorList error={query.error} />
      </>
    )
  }

  if (!progress) {
    return (
      <>
        <ScreenHeader title={exerciseName} />
        <LoadingState />
      </>
    )
  }

  const weights = progress.timeline.map((point) => point.weight ?? 0)
  const useReps = weights.every((weight) => weight === 0)
  const series = progress.timeline.map((point) => ({
    label: formatDay(point.date, language),
    value: useReps ? (point.reps ?? 0) : fromKilograms(point.weight ?? 0, settings.unit),
  }))

  return (
    <>
      <ScreenHeader title={progress.name} />

      <section className="stats-grid">
        <Stat label={t('stats.sessions')} value={String(progress.sessionCount)} />
        <Stat label={t('stats.totalSets')} value={String(progress.totalSets)} />
        <Stat
          label={useReps ? t('stats.bestReps') : t('stats.bestWeight')}
          value={
            useReps || progress.bestWeight === null
              ? String(progress.bestReps ?? '—')
              : `${fromKilograms(progress.bestWeight, settings.unit)} ${settings.unit}`
          }
        />
      </section>

      <section className="card">
        <h2 className="empty-title">{t('stats.timeline')}</h2>
        {series.length < 2 ? (
          <p className="list-row-meta">{t('stats.noTimeline')}</p>
        ) : (
          <LineChart points={series} />
        )}
      </section>

      <div className="card-list">
        {progress.firstSeen && (
          <article className="card list-row">
            <span className="list-row-meta">{t('stats.firstSeen')}</span>
            <span>{formatDay(progress.firstSeen, language)}</span>
          </article>
        )}
        {progress.lastSeen && (
          <article className="card list-row">
            <span className="list-row-meta">{t('stats.lastSeen')}</span>
            <span>{formatDay(progress.lastSeen, language)}</span>
          </article>
        )}
      </div>
    </>
  )
}

function Stat({ label, value }: { label: string; value: string }) {
  return (
    <article className="card stat-card">
      <span className="stat-value">{value}</span>
      <span className="stat-label">{label}</span>
    </article>
  )
}

type Point = {
  label: string
  value: number
}

const chartWidth = 320
const chartHeight = 140
const chartPadding = 12

function LineChart({ points }: { points: Point[] }) {
  const values = points.map((point) => point.value)
  const max = Math.max(...values)
  const min = Math.min(...values)
  const span = max - min || 1
  const step = (chartWidth - chartPadding * 2) / (points.length - 1)

  const coords = points.map((point, index) => ({
    x: chartPadding + index * step,
    y: chartHeight - chartPadding - ((point.value - min) / span) * (chartHeight - chartPadding * 2),
    ...point,
  }))

  const path = coords.map((coord) => `${coord.x},${coord.y}`).join(' ')

  return (
    <div className="chart">
      <svg viewBox={`0 0 ${chartWidth} ${chartHeight}`} role="img">
        <polyline className="chart-line" points={path} />
        {coords.map((coord) => (
          <circle key={`${coord.label}-${coord.x}`} className="chart-dot" cx={coord.x} cy={coord.y} r={3} />
        ))}
      </svg>
      <div className="chart-legend">
        <span>{min}</span>
        <span>{max}</span>
      </div>
    </div>
  )
}
