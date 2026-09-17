import { AreaChart, CalendarDays, ChevronRight, Dumbbell, History, ListChecks, Plus } from 'lucide-react'
import { fetchHome } from '@/api/home'
import { fetchRoutines } from '@/api/routines'
import { useAuth } from '@/auth/context'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'
import { describeApiError } from '@/i18n/apiErrors'
import { useI18n } from '@/i18n/context'
import { useApiQuery } from '@/hooks/useApiQuery'
import { useNavigation } from '@/navigation/context'

export function HomePage() {
  const { t } = useI18n()
  const { profile } = useAuth()
  const { navigate } = useNavigation()
  const home = useApiQuery('home', (token) => fetchHome(token))
  const routines = useApiQuery('routines', (token) => fetchRoutines(token))
  const routine = home.data?.currentRoutine ?? null
  const firstName = (profile.displayName || profile.email).split(/[\s@]/)[0]

  return (
    <div className="grid gap-6">
      <header>
        <h1 className="text-2xl font-semibold tracking-tight">
          {t('home.greeting', { name: firstName })}
        </h1>
      </header>

      {home.isLoading && <Skeleton className="h-44 w-full rounded-xl" />}

      {home.error !== null &&
        describeApiError(home.error, t).map((message) => (
          <p
            key={message}
            className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive"
          >
            {message}
          </p>
        ))}

      {!home.isLoading && home.error === null && routine && (
        <Card>
          <CardHeader>
            <CardDescription>{t('home.currentRoutineLabel')}</CardDescription>
            <CardTitle className="text-xl">{routine.name}</CardTitle>
          </CardHeader>
          <CardContent className="grid gap-5">
            <dl className="grid grid-cols-3 gap-3 text-center">
              <Metric value={routine.workoutCount} label={t('stats.workouts')} />
              <Metric value={routine.prCount} label={t('stats.prs')} />
              <Metric value={routine.weeksInUse} label={t('stats.weeks')} />
            </dl>
            <Button size="lg" className="h-12 w-full" onClick={() => navigate({ name: 'train' })}>
              <Dumbbell className="size-5" />
              {t('menu.train')}
            </Button>
          </CardContent>
        </Card>
      )}

      {!home.isLoading && home.error === null && !routine && (
        <Card>
          <CardContent className="grid gap-4 pt-6">
            <div>
              <p className="font-medium">{t('home.noRoutine')}</p>
              <p className="text-sm text-muted-foreground">{t('home.noRoutineBody')}</p>
            </div>
            <Button className="w-full" onClick={() => navigate({ name: 'routines' })}>
              <Plus className="size-4" />
              {t('routines.new')}
            </Button>
          </CardContent>
        </Card>
      )}

      <section className="grid gap-3">
        <div className="flex items-center justify-between">
          <h2 className="font-semibold">{t('menu.routines')}</h2>
          <Button variant="ghost" size="sm" onClick={() => navigate({ name: 'routines' })}>
            {t('routines.manage')}
          </Button>
        </div>

        {routines.isLoading && <Skeleton className="h-14 w-full rounded-xl" />}

        {routines.data?.length === 0 && (
          <p className="text-sm text-muted-foreground">{t('routines.emptyBody')}</p>
        )}

        <div className="grid gap-2">
          {routines.data?.slice(0, 3).map((item) => (
            <button
              key={item.id}
              type="button"
              className="flex items-center gap-3 rounded-xl border bg-card px-4 py-3 text-left transition-colors hover:bg-accent/40"
              onClick={() => navigate({ name: 'routines', expand: item.id })}
            >
              <ListChecks className="size-4 text-muted-foreground" />
              <span className="flex-1 truncate font-medium">{item.name}</span>
              {item.id === routine?.routineId && (
                <span className="rounded-full bg-accent px-2 py-0.5 text-xs font-medium text-accent-foreground">
                  {t('routines.current')}
                </span>
              )}
              <ChevronRight className="size-4 text-muted-foreground" />
            </button>
          ))}
        </div>
      </section>

      <section className="grid grid-cols-3 gap-3">
        <Shortcut
          icon={History}
          label={t('menu.history')}
          onClick={() => navigate({ name: 'history' })}
        />
        <Shortcut
          icon={AreaChart}
          label={t('menu.stats')}
          onClick={() => navigate({ name: 'stats' })}
        />
        <Shortcut
          icon={CalendarDays}
          label={t('menu.calendar')}
          onClick={() => navigate({ name: 'calendar' })}
        />
      </section>
    </div>
  )
}

function Metric({ value, label }: { value: number; label: string }) {
  return (
    <div className="grid gap-1">
      <dd className="text-2xl font-semibold text-primary">{value}</dd>
      <dt className="text-xs text-muted-foreground">{label}</dt>
    </div>
  )
}

function Shortcut({
  icon: Icon,
  label,
  onClick,
}: {
  icon: typeof History
  label: string
  onClick: () => void
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className="grid place-items-center gap-2 rounded-xl border bg-card py-4 text-xs font-medium transition-colors hover:bg-accent/40"
    >
      <Icon className="size-5 text-primary" />
      {label}
    </button>
  )
}
