import { Pause, Play, RotateCcw, SkipForward } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { formatSeconds, useIntervalTimer } from '@/hooks/useIntervalTimer'
import { useI18n } from '@/i18n/context'
import { useSettings } from '@/settings/context'

export function FloatingTimer() {
  const { t } = useI18n()
  const { settings } = useSettings()
  const timer = useIntervalTimer(settings)

  const phaseLabel = {
    idle: t('train.prep'),
    prep: t('train.prep'),
    work: t('train.work'),
    rest: t('train.rest'),
    done: t('train.done'),
  }[timer.phase]

  const accent =
    timer.phase === 'work'
      ? 'text-primary'
      : timer.phase === 'rest'
        ? 'text-destructive'
        : 'text-muted-foreground'

  return (
    <div className="pointer-events-none fixed inset-x-0 bottom-16 z-20 px-4 md:bottom-4">
      <div className="pointer-events-auto mx-auto flex max-w-md items-center gap-3 rounded-2xl border bg-card/95 px-4 py-2.5 shadow-lg backdrop-blur">
        <div className="grid">
          <span className={`text-[0.65rem] font-semibold uppercase tracking-wide ${accent}`}>
            {phaseLabel}
          </span>
          <span className="text-2xl font-semibold leading-none tabular-nums">
            {formatSeconds(timer.remaining)}
          </span>
        </div>

        <span className="text-xs text-muted-foreground">
          {t('train.currentSet', { current: timer.set, total: timer.total })}
        </span>

        <div className="ml-auto flex items-center gap-1">
          <Button
            size="icon"
            variant="ghost"
            aria-label={t('train.timerSkip')}
            onClick={timer.skip}
          >
            <SkipForward className="size-4" />
          </Button>

          <Button
            size="icon"
            className="size-11 rounded-full"
            aria-label={timer.running ? t('train.timerStop') : t('train.timerStart')}
            onClick={timer.running ? timer.pause : timer.start}
          >
            {timer.running ? <Pause className="size-5" /> : <Play className="size-5" />}
          </Button>

          <Button
            size="icon"
            variant="ghost"
            aria-label={t('common.reset')}
            onClick={timer.reset}
          >
            <RotateCcw className="size-4" />
          </Button>
        </div>
      </div>
    </div>
  )
}
