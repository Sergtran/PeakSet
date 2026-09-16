import { createContext, useContext } from 'react'
import type { Settings } from './types'

export type SettingsValue = {
  settings: Settings
  isLoading: boolean
  error: unknown
  update: (patch: Partial<Settings>) => void
}

export const SettingsContext = createContext<SettingsValue | null>(null)

export function useSettings(): SettingsValue {
  const value = useContext(SettingsContext)
  if (!value) {
    throw new Error('useSettings must be used inside SettingsProvider')
  }
  return value
}
