import { CalendarDays, Dumbbell, History, Home, ListChecks } from 'lucide-react'
import { useAuth } from '@/auth/context'
import { Button } from '@/components/ui/button'
import { useI18n } from '@/i18n/context'
import { useNavigation, type Route } from '@/navigation/context'
import { CalendarPage } from '@/pages/CalendarPage'
import { ExercisePage } from '@/pages/ExercisePage'
import { HistoryPage } from '@/pages/HistoryPage'
import { HomePage } from '@/pages/HomePage'
import { RoutinePage } from '@/pages/RoutinePage'
import { RoutinesPage } from '@/pages/RoutinesPage'
import { SessionPage } from '@/pages/SessionPage'
import { SettingsPage } from '@/pages/SettingsPage'
import { StatsPage } from '@/pages/StatsPage'
import { TrainPage } from '@/pages/TrainPage'
import { WorkoutPage } from '@/pages/WorkoutPage'

type NavItem = {
  name: Route['name']
  labelKey: Parameters<ReturnType<typeof useI18n>['t']>[0]
  icon: typeof Home
}

const navItems: NavItem[] = [
  { name: 'menu', labelKey: 'menu.home', icon: Home },
  { name: 'train', labelKey: 'menu.train', icon: Dumbbell },
  { name: 'routines', labelKey: 'menu.routines', icon: ListChecks },
  { name: 'history', labelKey: 'menu.history', icon: History },
  { name: 'calendar', labelKey: 'menu.calendar', icon: CalendarDays },
]

const children: Partial<Record<Route['name'], Route['name'][]>> = {
  routines: ['routine'],
  history: ['workout'],
  train: ['session'],
}

function isCurrent(current: Route['name'], item: Route['name']) {
  return current === item || (children[item] ?? []).includes(current)
}

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
  }
}

export function AppShell() {
  const { t } = useI18n()
  const { profile } = useAuth()
  const { route, navigate } = useNavigation()

  const initials = (profile.displayName || profile.email).trim().slice(0, 1).toUpperCase()

  return (
    <div className="flex min-h-svh flex-col">
      <header className="sticky top-0 z-10 border-b bg-card/80 backdrop-blur">
        <div className="mx-auto flex h-14 w-full max-w-3xl items-center gap-2 px-4">
          <button
            type="button"
            className="flex items-center gap-2"
            onClick={() => navigate({ name: 'menu' })}
          >
            <img src="/logo-192.png" alt="" width={28} height={28} className="size-7 rounded-lg" />
            <span className="text-base font-semibold">{t('app.name')}</span>
          </button>

          <nav className="ml-4 hidden items-center gap-1 md:flex">
            {navItems.map((item) => {
              const Icon = item.icon
              const active = isCurrent(route.name, item.name)
              return (
                <Button
                  key={item.name}
                  variant={active ? 'secondary' : 'ghost'}
                  size="sm"
                  onClick={() => navigate({ name: item.name } as Route)}
                >
                  <Icon className="size-4" />
                  {t(item.labelKey)}
                </Button>
              )
            })}
          </nav>

          <Button
            variant="ghost"
            size="icon"
            className="ml-auto rounded-full bg-accent text-accent-foreground"
            aria-label={t('settings.title')}
            onClick={() => navigate({ name: 'settings' })}
          >
            <span className="text-sm font-semibold">{initials}</span>
          </Button>
        </div>
      </header>

      <main className="mx-auto w-full max-w-3xl flex-1 px-4 py-5 pb-24 md:pb-10">
        {renderRoute(route)}
      </main>

      <nav className="fixed inset-x-0 bottom-0 z-10 border-t bg-card/95 backdrop-blur md:hidden">
        <div className="mx-auto grid max-w-md grid-cols-5">
          {navItems.map((item) => {
            const Icon = item.icon
            const active = isCurrent(route.name, item.name)
            return (
              <button
                key={item.name}
                type="button"
                onClick={() => navigate({ name: item.name } as Route)}
                className={
                  active
                    ? 'flex flex-col items-center gap-1 py-2.5 text-xs font-medium text-primary'
                    : 'flex flex-col items-center gap-1 py-2.5 text-xs text-muted-foreground'
                }
              >
                <Icon className="size-5" />
                {t(item.labelKey)}
              </button>
            )
          })}
        </div>
      </nav>
    </div>
  )
}
