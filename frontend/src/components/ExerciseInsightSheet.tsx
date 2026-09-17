import { fetchExerciseProgress } from '@/api/exercises'
import { ProgressChart } from '@/components/ProgressChart'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet'
import { useApiQuery } from '@/hooks/useApiQuery'
import { useI18n } from '@/i18n/context'
import { useSettings } from '@/settings/context'
import { formatDay } from '@/utils/format'
import { fromKilograms } from '@/utils/units'

export type InsightView = 'history' | 'chart'

type Props = {
  exercise: { name: string; view: InsightView } | null
  onClose: () => void
}

export function ExerciseInsightSheet({ exercise, onClose }: Props) {
  const { t, language } = useI18n()
  const { settings } = useSettings()
  const name = exercise?.name ?? ''
  const query = useApiQuery(`exercise:${name || 'none'}`, (token) =>
    name ? fetchExerciseProgress(token, name) : Promise.resolve(null),
  )
  const progress = query.data

  const points = (progress?.timeline ?? []).map((point) => ({
    label: formatDay(point.date, language),
    value: fromKilograms(point.weight ?? 0, settings.unit),
  }))

  return (
    <Sheet open={exercise !== null} onOpenChange={(open) => !open && onClose()}>
      <SheetContent className="w-full overflow-y-auto sm:max-w-md">
        <SheetHeader>
          <SheetTitle className="pr-8">{name}</SheetTitle>
          <SheetDescription>
            {exercise?.view === 'chart' ? t('stats.timeline') : t('stats.sessions')}
          </SheetDescription>
        </SheetHeader>

        <div className="grid gap-5 px-4 pb-6">
          {query.isLoading && <Skeleton className="h-32 w-full rounded-xl" />}

          {progress && (
            <>
              <dl className="grid grid-cols-3 gap-3 text-center">
                <Stat value={String(progress.sessionCount)} label={t('stats.sessions')} />
                <Stat value={String(progress.totalSets)} label={t('stats.totalSets')} />
                <Stat
                  value={
                    progress.bestWeight === null
                      ? String(progress.bestReps ?? '—')
                      : `${fromKilograms(progress.bestWeight, settings.unit)} ${settings.unit}`
                  }
                  label={progress.bestWeight === null ? t('stats.bestReps') : t('stats.bestWeight')}
                />
              </dl>

              {exercise?.view === 'chart' && (
                <section className="grid gap-2">
                  {points.length < 2 ? (
                    <p className="text-sm text-muted-foreground">{t('stats.noTimeline')}</p>
                  ) : (
                    <>
                      <ProgressChart points={points} unit={settings.unit} />
                      <p className="text-xs text-muted-foreground">{t('stats.timelineNote')}</p>
                    </>
                  )}
                </section>
              )}

              {exercise?.view === 'history' && (
                <ol className="grid gap-2">
                  {[...progress.timeline].reverse().slice(0, 12).map((point) => (
                    <li
                      key={`${point.date}-${point.weight}-${point.reps}`}
                      className="flex items-center gap-3 rounded-lg border px-3 py-2 text-sm"
                    >
                      <span className="flex-1 text-muted-foreground">
                        {formatDay(point.date, language)}
                      </span>
                      <span className="tabular-nums">
                        {point.reps ?? '—'} ×{' '}
                        {point.weight === null
                          ? '—'
                          : `${fromKilograms(point.weight, settings.unit)} ${settings.unit}`}
                      </span>
                      {point.prStatus !== null && (
                        <Badge variant="secondary">{t('workout.pr')}</Badge>
                      )}
                    </li>
                  ))}
                </ol>
              )}
            </>
          )}
        </div>
      </SheetContent>
    </Sheet>
  )
}

function Stat({ value, label }: { value: string; label: string }) {
  return (
    <div className="grid gap-1">
      <dd className="text-xl font-semibold text-primary">{value}</dd>
      <dt className="text-xs text-muted-foreground">{label}</dt>
    </div>
  )
}
