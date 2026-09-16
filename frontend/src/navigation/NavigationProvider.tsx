import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from 'react'
import { NavigationContext, type NavigationValue, type Route } from './context'

type HistoryEntry = { stack: Route[] }

const root: Route = { name: 'menu' }
const initialStack: Route[] = [root]

function readEntry(state: unknown): Route[] {
  const entry = state as HistoryEntry | null
  return Array.isArray(entry?.stack) && entry.stack.length > 0 ? entry.stack : [root]
}

export function NavigationProvider({ children }: { children: ReactNode }) {
  const stackRef = useRef<Route[]>(initialStack)
  const [stack, setStack] = useState<Route[]>(initialStack)

  useEffect(() => {
    window.history.replaceState({ stack: stackRef.current } satisfies HistoryEntry, '')

    function handlePopState(event: PopStateEvent) {
      const next = readEntry(event.state)
      stackRef.current = next
      setStack(next)
    }

    window.addEventListener('popstate', handlePopState)
    return () => window.removeEventListener('popstate', handlePopState)
  }, [])

  const navigate = useCallback((next: Route) => {
    const nextStack = [...stackRef.current, next]
    stackRef.current = nextStack
    setStack(nextStack)
    window.history.pushState({ stack: nextStack } satisfies HistoryEntry, '')
  }, [])

  // The browser owns the back action: it walks its own history, and popstate
  // gives us back the stack that was stored with each entry.
  const back = useCallback(() => {
    if (stackRef.current.length > 1) {
      window.history.back()
    }
  }, [])

  const value = useMemo<NavigationValue>(
    () => ({ route: stack[stack.length - 1], navigate, back, canGoBack: stack.length > 1 }),
    [stack, navigate, back],
  )

  return <NavigationContext.Provider value={value}>{children}</NavigationContext.Provider>
}
