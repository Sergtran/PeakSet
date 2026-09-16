import { apiRequest } from './client'

export type UserSettings = {
  theme: 'Light' | 'Dark' | 'System'
  timerPrepSeconds: number
  timerWorkSeconds: number
  timerRestSeconds: number
  timerSets: number
  currentRoutineId: string | null
}

export type UserSettingsRequest = {
  theme: 'Light' | 'Dark' | 'System'
  timerPrepSeconds: number
  timerWorkSeconds: number
  timerRestSeconds: number
  timerSets: number
}

export function fetchSettings(token: string): Promise<UserSettings> {
  return apiRequest<UserSettings>('/users/me/settings', { token })
}

export function saveSettings(token: string, request: UserSettingsRequest): Promise<UserSettings> {
  return apiRequest<UserSettings>('/users/me/settings', { method: 'PUT', body: request, token })
}
