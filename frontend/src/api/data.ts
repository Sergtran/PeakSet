import { apiRequest } from './client'
import type { ExerciseType, Laterality, PrStatus } from './types'

export type LiftrazaExport = {
  version: number
  exportedAtUtc: string
  settings: {
    theme: 'Light' | 'Dark'
    timerPrepSeconds: number
    timerWorkSeconds: number
    timerRestSeconds: number
    timerSets: number
    currentRoutineId: string | null
  }
  routines: {
    id: string
    name: string
    sessions: {
      id: string
      name: string
      displayOrder: number
      exercises: {
        id: string
        name: string
        exerciseType: ExerciseType
        laterality: Laterality
        displayOrder: number
      }[]
    }[]
  }[]
  workouts: {
    id: string
    routineId: string | null
    routineName: string
    sessionName: string
    workoutDate: string
    exercises: {
      id: string
      name: string
      exerciseType: ExerciseType
      laterality: Laterality
      prStatus: PrStatus | null
      displayOrder: number
      sets: { id: string; setNumber: number; reps: number; weight: number }[]
    }[]
  }[]
}

export function exportData(token: string): Promise<LiftrazaExport> {
  return apiRequest<LiftrazaExport>('/data/export', { token })
}

export function importData(token: string, payload: LiftrazaExport): Promise<void> {
  return apiRequest<void>('/data/import', { method: 'POST', body: payload, token })
}
