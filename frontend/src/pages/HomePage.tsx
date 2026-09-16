import { fetchHome } from '../api/home'
import { EmptyState, ErrorList, LoadingState } from '../components/Feedback'
import { useApiQuery } from '../hooks/useApiQuery'
import { useI18n } from '../i18n/context'
import type { TranslationKey } from '../i18n/translations'
import { useNavigation } from '../navigation/context'

type SimpleRoute =
  | { name: 'train' }
  | { name: 'routines' }
  | { name: 'history' }
  | { name: 'calendar' }
  | { name: 'stats' }
  | { name: 'settings' }
  | { name: 'data' }

type Entry = {
  route: SimpleRoute
  title: TranslationKey
  hint: TranslationKey
}

const entries: Entry[] = [
  { route: { name: 'train' }, title: 'menu.train', hint: 'menu.trainHint' },
  { route: { name: 'routines' }, title: 'menu.routines', hint: 'menu.routinesHint' },
  { route: { name: 'history' }, title: 'menu.history', hint: 'menu.historyHint' },
  { route: { name: 'calendar' }, title: 'menu.calendar', hint: 'menu.calendarHint' },
  { route: { name: 'stats' }, title: 'menu.stats', hint: 'menu.statsHint' },
  { route: { name: 'settings' }, title: 'menu.settings', hint: 'menu.settingsHint' },
  { route: { name: 'data' }, title: 'menu.data', hint: 'menu.dataHint' },
]

export function HomePage() {
  const { t } = useI18n()
  const { navigate } = useNavigation()
  const home = useApiQuery('home', (token) => fetchHome(token))
  const routine = home.data?.currentRoutine ?? null

  return (
    <>
      <h1 className="page-title">
        {routine ? t('home.currentRoutine', { name: routine.name }) : t('app.name')}
      </h1>

      {home.isLoading && <LoadingState />}
      {home.error !== null && <ErrorList error={home.error} />}

      {routine && (
        <section className="stats-grid">
          <Stat label={t('stats.workouts')} value={routine.workoutCount} />
          <Stat label={t('stats.prs')} value={routine.prCount} />
          <Stat label={t('stats.weeks')} value={routine.weeksInUse} />
          {routine.workoutCount > 0 && (
            <Stat
              label={t('stats.daysSince')}
              value={routine.daysSinceLastWorkout}
              highlight={routine.daysSinceLastWorkout >= 7}
            />
          )}
        </section>
      )}

      {!home.isLoading && home.error === null && !routine && (
        <EmptyState title={t('home.noRoutine')} body={t('home.noRoutineBody')} />
      )}

      <nav className="menu-grid">
        {entries.map((entry) => (
          <button
            key={entry.route.name}
            type="button"
            className="menu-card"
            onClick={() => navigate(entry.route)}
          >
            <span className="menu-card-title">{t(entry.title)}</span>
            <span className="menu-card-hint">{t(entry.hint)}</span>
          </button>
        ))}
      </nav>
    </>
  )
}

function Stat({ label, value, highlight = false }: { label: string; value: number; highlight?: boolean }) {
  return (
    <article className={highlight ? 'card stat-card stat-card-warn' : 'card stat-card'}>
      <span className="stat-value">{value}</span>
      <span className="stat-label">{label}</span>
    </article>
  )
}
