import { createContext, useContext } from 'react'

export type Route =
  | { name: 'menu' }
  | { name: 'train' }
  | { name: 'session'; routineId: string; sessionId: string; sessionName: string }
  | { name: 'routines'; expand?: string }
  | { name: 'history' }
  | { name: 'workout'; workoutId: string }
  | { name: 'calendar' }
  | { name: 'stats' }
  | { name: 'exercise'; exerciseName: string }
  | { name: 'settings' }

export type RouteName = Route['name']

export type NavigationValue = {
  route: Route
  navigate: (route: Route) => void
  back: () => void
  canGoBack: boolean
}

export const NavigationContext = createContext<NavigationValue | null>(null)

export function useNavigation(): NavigationValue {
  const value = useContext(NavigationContext)
  if (!value) {
    throw new Error('useNavigation must be used inside NavigationProvider')
  }
  return value
}
