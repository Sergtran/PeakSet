import { apiRequest } from './client'
import type { PrStatus } from './types'

export type ExerciseSummary = {
  name: string
  sessionCount: number
  lastUsed: string | null
}

export type ProgressPoint = {
  date: string
  weight: number | null
  reps: number | null
  prStatus: PrStatus | null
}

export type ExerciseProgress = {
  name: string
  sessionCount: number
  totalSets: number
  bestWeight: number | null
  bestReps: number | null
  firstSeen: string | null
  lastSeen: string | null
  timeline: ProgressPoint[]
}

export function fetchExercises(token: string): Promise<ExerciseSummary[]> {
  return apiRequest<ExerciseSummary[]>('/exercises', { token })
}

export function fetchExerciseProgress(
  token: string,
  name: string,
): Promise<ExerciseProgress> {
  return apiRequest<ExerciseProgress>(`/exercises/${encodeURIComponent(name)}/progress`, { token })
}
