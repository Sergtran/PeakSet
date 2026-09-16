import { useCallback, useEffect, useState } from 'react'
import { useI18n } from '../i18n/context'
import type { Settings } from '../settings/types'

type Phase = 'idle' | 'prep' | 'work' | 'rest' | 'done'

type TimerState = {
  running: boolean
  phase: Phase
  set: number
  remaining: number
}

function initialState(prepSeconds: number): TimerState {
  return { running: false, phase: 'idle', set: 1, remaining: prepSeconds }
}

function advance(state: TimerState, config: Settings): TimerState {
  if (!state.running || state.phase === 'done' || state.phase === 'idle') {
    return state
  }
  if (state.remaining > 1) {
    return { ...state, remaining: state.remaining - 1 }
  }
  if (state.phase === 'prep') {
    return { ...state, phase: 'work', remaining: config.workSeconds }
  }
  if (state.phase === 'rest') {
    return { ...state, phase: 'work', set: state.set + 1, remaining: config.workSeconds }
  }
  if (state.set < config.sets) {
    return { ...state, phase: 'rest', remaining: config.restSeconds }
  }
  return { ...state, running: false, phase: 'done', remaining: 0 }
}

function formatSeconds(total: number): string {
  const minutes = Math.floor(total / 60)
  const seconds = total % 60
  return `${minutes}:${String(seconds).padStart(2, '0')}`
}

export function IntervalTimer({ config }: { config: Settings }) {
  const { t } = useI18n()
  const [state, setState] = useState<TimerState>(() => initialState(config.prepSeconds))

  const tick = useCallback(() => {
    setState((current) => advance(current, config))
  }, [config])

  useEffect(() => {
    if (!state.running) return
    const id = setInterval(tick, 1000)
    return () => clearInterval(id)
  }, [state.running, tick])

  const phaseLabels = {
    idle: t('train.prep'),
    prep: t('train.prep'),
    work: t('train.work'),
    rest: t('train.rest'),
    done: t('train.done'),
  } satisfies Record<Phase, string>

  function start() {
    setState({ running: true, phase: 'prep', set: 1, remaining: config.prepSeconds })
  }

  function stop() {
    setState(initialState(config.prepSeconds))
  }

  return (
    <section className={`card timer timer-${state.phase}`}>
      <span className="timer-phase">{phaseLabels[state.phase]}</span>
      <span className="timer-display">{formatSeconds(state.remaining)}</span>
      <span className="timer-set">
        {t('train.currentSet', { current: state.set, total: config.sets })}
      </span>
      <div className="button-row">
        {state.running ? (
          <>
            <button className="btn btn-ghost" type="button" onClick={tick}>
              {t('train.timerSkip')}
            </button>
            <button className="btn btn-danger" type="button" onClick={stop}>
              {t('train.timerStop')}
            </button>
          </>
        ) : (
          <button className="btn btn-primary" type="button" onClick={start}>
            {t('train.timerStart')}
          </button>
        )}
      </div>
    </section>
  )
}
