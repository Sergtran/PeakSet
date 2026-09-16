import { apiRequest } from './client'
import type { ExerciseType, Laterality, PrStatus } from './types'

export type WorkoutSet = {
  setNumber: number
  reps: number
  weight: number
}

export type WorkoutExercise = {
  id: string
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
  prStatus: PrStatus | null
  sets: WorkoutSet[]
}

export type Workout = {
  id: string
  routineId: string | null
  routineName: string
  sessionName: string
  workoutDate: string
  createdAt: string
  exercises: WorkoutExercise[]
}

export type PagedResult<T> = {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export type WorkoutSetRequest = {
  reps: number
  weight: number
}

export type WorkoutExerciseRequest = {
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
  sets: WorkoutSetRequest[]
}

export type WorkoutRequest = {
  routineId: string | null
  routineName: string
  sessionName: string
  workoutDate: string
  exercises: WorkoutExerciseRequest[]
}

export function createWorkout(token: string, request: WorkoutRequest): Promise<Workout> {
  return apiRequest<Workout>('/workouts', { method: 'POST', body: request, token })
}

export function fetchWorkouts(
  token: string,
  page: number,
  pageSize: number,
): Promise<PagedResult<Workout>> {
  return apiRequest<PagedResult<Workout>>(`/workouts?page=${page}&pageSize=${pageSize}`, { token })
}

export function fetchWorkout(token: string, workoutId: string): Promise<Workout> {
  return apiRequest<Workout>(`/workouts/${workoutId}`, { token })
}

export function updateWorkout(
  token: string,
  workoutId: string,
  request: WorkoutRequest,
): Promise<Workout> {
  return apiRequest<Workout>(`/workouts/${workoutId}`, { method: 'PUT', body: request, token })
}

export function deleteWorkout(token: string, workoutId: string): Promise<void> {
  return apiRequest<void>(`/workouts/${workoutId}`, { method: 'DELETE', token })
}
