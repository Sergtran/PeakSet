import { apiRequest } from './client'
import type { ExerciseType, Laterality } from './types'

export type Routine = {
  id: string
  name: string
  createdAt: string
}

export type SessionExercise = {
  id: string
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
  displayOrder: number
}

export type Session = {
  id: string
  name: string
  displayOrder: number
  exercises: SessionExercise[]
}

export type RoutineDetail = {
  id: string
  name: string
  createdAt: string
  sessions: Session[]
}

export type SessionExerciseRequest = {
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
}

export type RoutineStats = {
  routineId: string
  name: string
  workoutCount: number
  firstWorkoutDate: string | null
  lastWorkoutDate: string | null
  daysSinceLastWorkout: number
  weeksInUse: number
  prCount: number
  lastWeeks: { weekStart: string; workoutCount: number }[]
}

export type RoutineUsage = {
  routineId: string
  name: string
  periods: { start: string; end: string | null; workoutCount: number }[]
}

export type ExerciseUsage = {
  name: string
  sessionCount: number
}

export function fetchRoutines(token: string): Promise<Routine[]> {
  return apiRequest<Routine[]>('/routines', { token })
}

export function fetchRoutine(token: string, routineId: string): Promise<RoutineDetail> {
  return apiRequest<RoutineDetail>(`/routines/${routineId}`, { token })
}

export function createRoutine(token: string, name: string): Promise<Routine> {
  return apiRequest<Routine>('/routines', { method: 'POST', body: { name }, token })
}

export function renameRoutine(token: string, routineId: string, name: string): Promise<Routine> {
  return apiRequest<Routine>(`/routines/${routineId}`, { method: 'PUT', body: { name }, token })
}

export function deleteRoutine(token: string, routineId: string): Promise<void> {
  return apiRequest<void>(`/routines/${routineId}`, { method: 'DELETE', token })
}

export function createSession(token: string, routineId: string, name: string): Promise<Session> {
  return apiRequest<Session>(`/routines/${routineId}/sessions`, {
    method: 'POST',
    body: { name },
    token,
  })
}

export function renameSession(
  token: string,
  routineId: string,
  sessionId: string,
  name: string,
): Promise<Session> {
  return apiRequest<Session>(`/routines/${routineId}/sessions/${sessionId}`, {
    method: 'PUT',
    body: { name },
    token,
  })
}

export function deleteSession(
  token: string,
  routineId: string,
  sessionId: string,
): Promise<void> {
  return apiRequest<void>(`/routines/${routineId}/sessions/${sessionId}`, {
    method: 'DELETE',
    token,
  })
}

export function createSessionExercise(
  token: string,
  routineId: string,
  sessionId: string,
  request: SessionExerciseRequest,
): Promise<SessionExercise> {
  return apiRequest<SessionExercise>(`/routines/${routineId}/sessions/${sessionId}/exercises`, {
    method: 'POST',
    body: request,
    token,
  })
}

export function updateSessionExercise(
  token: string,
  routineId: string,
  sessionId: string,
  exerciseId: string,
  request: SessionExerciseRequest,
): Promise<SessionExercise> {
  return apiRequest<SessionExercise>(
    `/routines/${routineId}/sessions/${sessionId}/exercises/${exerciseId}`,
    { method: 'PUT', body: request, token },
  )
}

export function deleteSessionExercise(
  token: string,
  routineId: string,
  sessionId: string,
  exerciseId: string,
): Promise<void> {
  return apiRequest<void>(
    `/routines/${routineId}/sessions/${sessionId}/exercises/${exerciseId}`,
    { method: 'DELETE', token },
  )
}

export function fetchRoutineStats(token: string, routineId: string): Promise<RoutineStats> {
  return apiRequest<RoutineStats>(`/routines/${routineId}/stats`, { token })
}

export function fetchRoutineUsage(token: string, routineId: string): Promise<RoutineUsage> {
  return apiRequest<RoutineUsage>(`/routines/${routineId}/usage`, { token })
}

export function fetchTopExercises(
  token: string,
  routineId: string,
  limit = 10,
): Promise<ExerciseUsage[]> {
  return apiRequest<ExerciseUsage[]>(`/routines/${routineId}/exercises/top?limit=${limit}`, {
    token,
  })
}
