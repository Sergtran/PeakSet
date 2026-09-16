import { apiRequest } from './client'

export function setCurrentRoutine(token: string, routineId: string | null): Promise<void> {
  return apiRequest<void>('/users/me/current-routine', {
    method: 'PUT',
    body: { routineId },
    token,
  })
}
