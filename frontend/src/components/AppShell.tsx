import { useAuth } from '../auth/context'
import { useI18n } from '../i18n/context'
import { useNavigation, type Route } from '../navigation/context'
import { CalendarPage } from '../pages/CalendarPage'
import { DataPage } from '../pages/DataPage'
import { ExercisePage } from '../pages/ExercisePage'
import { HistoryPage } from '../pages/HistoryPage'
import { HomePage } from '../pages/HomePage'
import { RoutinePage } from '../pages/RoutinePage'
import { RoutinesPage } from '../pages/RoutinesPage'
import { SessionPage } from '../pages/SessionPage'
import { SettingsPage } from '../pages/SettingsPage'
import { StatsPage } from '../pages/StatsPage'
import { TrainPage } from '../pages/TrainPage'
import { WorkoutPage } from '../pages/WorkoutPage'
import { LanguageSwitcher } from './LanguageSwitcher'

function renderRoute(route: Route) {
  switch (route.name) {
    case 'menu':
      return <HomePage />
    case 'train':
      return <TrainPage />
    case 'session':
      return (
        <SessionPage
          routineId={route.routineId}
          sessionId={route.sessionId}
          sessionName={route.sessionName}
        />
      )
    case 'routines':
      return <RoutinesPage />
    case 'routine':
      return <RoutinePage routineId={route.routineId} />
    case 'history':
      return <HistoryPage />
    case 'workout':
      return <WorkoutPage workoutId={route.workoutId} />
    case 'calendar':
      return <CalendarPage />
    case 'stats':
      return <StatsPage />
    case 'exercise':
      return <ExercisePage exerciseName={route.exerciseName} />
    case 'settings':
      return <SettingsPage />
    case 'data':
      return <DataPage />
  }
}

export function AppShell() {
  const { t } = useI18n()
  const { profile, signOut } = useAuth()
  const { route, navigate } = useNavigation()

  return (
    <div className="app-shell">
      <header className="app-header">
        <button type="button" className="app-header-brand" onClick={() => navigate({ name: 'menu' })}>
          <img
            className="brand-mark brand-mark-sm"
            src="/logo-192.png"
            alt=""
            width={32}
            height={32}
          />
          <span className="brand-name brand-name-sm">{t('app.name')}</span>
        </button>

        <div className="app-header-actions">
          <LanguageSwitcher />
          <span className="user-name">{profile.displayName || profile.email}</span>
          <button className="btn btn-ghost" type="button" onClick={signOut}>
            {t('menu.signOut')}
          </button>
        </div>
      </header>

      <main className="app-main">{renderRoute(route)}</main>
    </div>
  )
}
