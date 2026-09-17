import { useEffect } from 'react'

/**
 * Keeps the screen awake while a workout is open. The browser releases the lock
 * when the tab goes to the background, so it is requested again on return.
 */
export function useWakeLock(active: boolean) {
  useEffect(() => {
    if (!active || !('wakeLock' in navigator)) return

    let sentinel: WakeLockSentinel | null = null
    let cancelled = false

    async function request() {
      try {
        const lock = await navigator.wakeLock.request('screen')
        if (cancelled) {
          await lock.release()
          return
        }
        sentinel = lock
      } catch {
        // The browser can refuse (low battery, unsupported). Nothing to do.
      }
    }

    function handleVisibility() {
      if (document.visibilityState === 'visible' && sentinel === null) {
        void request()
      }
    }

    void request()
    document.addEventListener('visibilitychange', handleVisibility)

    return () => {
      cancelled = true
      document.removeEventListener('visibilitychange', handleVisibility)
      void sentinel?.release()
    }
  }, [active])
}
