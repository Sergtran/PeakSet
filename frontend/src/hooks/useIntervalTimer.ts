import { useCallback, useEffect, useState } from 'react'
import type { Settings } from '@/settings/types'

export type TimerPhase = 'idle' | 'prep' | 'work' | 'rest' | 'done'

type TimerState = {
  running: boolean
  phase: TimerPhase
  set: number
  remaining: number
}

function initialState(prepSeconds: number): TimerState {
  return { running: false, phase: 'idle', set: 1, remaining: prepSeconds }
}

function advance(state: TimerState, config: Settings): TimerState {
  if (!state.running || state.phase === 'idle' || state.phase === 'done') {
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

export function formatSeconds(total: number): string {
  const minutes = Math.floor(total / 60)
  const seconds = total % 60
  return `${minutes}:${String(seconds).padStart(2, '0')}`
}

/**
 * Interval timer as a pure state machine: `advance` receives the current state
 * and returns the next one, so the ticking effect stays side-effect free.
 */
export function useIntervalTimer(config: Settings) {
  const [state, setState] = useState<TimerState>(() => initialState(config.prepSeconds))

  const tick = useCallback(() => {
    setState((current) => advance(current, config))
  }, [config])

  useEffect(() => {
    if (!state.running) return
    const id = setInterval(tick, 1000)
    return () => clearInterval(id)
  }, [state.running, tick])

  const start = useCallback(() => {
    setState((current) =>
      current.phase === 'idle' || current.phase === 'done'
        ? { running: true, phase: 'prep', set: 1, remaining: config.prepSeconds }
        : { ...current, running: true },
    )
  }, [config.prepSeconds])

  const pause = useCallback(() => {
    setState((current) => ({ ...current, running: false }))
  }, [])

  const reset = useCallback(() => {
    setState(initialState(config.prepSeconds))
  }, [config.prepSeconds])

  const skip = useCallback(() => {
    setState((current) => advance({ ...current, running: true, remaining: 1 }, config))
  }, [config])

  return { ...state, total: config.sets, start, pause, reset, skip }
}
