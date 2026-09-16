import { useState } from 'react'
import { fetchCalendarMonth } from '../api/calendar'
import { ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'
import { dateKey, formatMonth, todayKey } from '../utils/format'

export function CalendarPage() {
  const { t, language } = useI18n()
  const { navigate } = useNavigation()
  const now = new Date()
  const [year, setYear] = useState(now.getFullYear())
  const [month, setMonth] = useState(now.getMonth() + 1)
  const [selected, setSelected] = useState<string | null>(null)
  const query = useApiQuery(`calendar:${year}-${month}`, (token) =>
    fetchCalendarMonth(token, year, month),
  )

  const daysInMonth = new Date(year, month, 0).getDate()
  const leadingBlanks = (new Date(year, month - 1, 1).getDay() + 6) % 7
  const today = todayKey()

  const byDay = new Map<string, { id: string; label: string }[]>()
  for (const day of query.data?.days ?? []) {
    byDay.set(
      day.date.slice(0, 10),
      day.workouts.map((workout) => ({
        id: workout.id,
        label: `${workout.routineName} · ${workout.sessionName}`,
      })),
    )
  }

  function shiftMonth(delta: number) {
    const next = new Date(year, month - 1 + delta, 1)
    setYear(next.getFullYear())
    setMonth(next.getMonth() + 1)
    setSelected(null)
  }

  const selectedWorkouts = selected ? (byDay.get(selected) ?? []) : []

  return (
    <>
      <ScreenHeader title={t('menu.calendar')} subtitle={formatMonth(year, month, language)} />

      <div className="button-row pagination">
        <button className="btn btn-ghost" type="button" onClick={() => shiftMonth(-1)}>
          ←
        </button>
        <button className="btn btn-ghost" type="button" onClick={() => shiftMonth(1)}>
          →
        </button>
      </div>

      {query.isLoading && <LoadingState />}
      {query.error !== null && <ErrorList error={query.error} />}

      {query.data && (
        <section className="card calendar">
          <div className="calendar-grid">
            {Array.from({ length: leadingBlanks }, (_, index) => (
              <span key={`blank-${index}`} className="calendar-cell calendar-blank" />
            ))}
            {Array.from({ length: daysInMonth }, (_, index) => {
              const day = index + 1
              const key = dateKey(year, month, day)
              const workouts = byDay.get(key) ?? []
              const classes = ['calendar-cell']
              if (workouts.length > 0) classes.push('calendar-has-workout')
              if (key === today) classes.push('calendar-today')
              if (key === selected) classes.push('calendar-selected')
              return (
                <button
                  key={key}
                  type="button"
                  className={classes.join(' ')}
                  onClick={() => setSelected(workouts.length > 0 ? key : null)}
                >
                  <span className="calendar-day">{day}</span>
                  {workouts.length > 0 && <span className="calendar-dot" />}
                </button>
              )
            })}
          </div>

          {selected && (
            <div className="calendar-detail">
              <h2 className="list-row-title">{selected}</h2>
              {selectedWorkouts.length === 0 && (
                <p className="list-row-meta">{t('calendar.noWorkouts')}</p>
              )}
              {selectedWorkouts.map((workout) => (
                <button
                  key={workout.id}
                  type="button"
                  className="btn btn-ghost"
                  onClick={() => navigate({ name: 'workout', workoutId: workout.id })}
                >
                  {workout.label}
                </button>
              ))}
            </div>
          )}
        </section>
      )}
    </>
  )
}
