import { apiRequest } from './client'

/** Summary of the routine the user marked as current. */
export type RoutineOverview = {
  routineId: string
  name: string
  workoutCount: number
  firstWorkoutDate: string | null
  lastWorkoutDate: string | null
  daysSinceLastWorkout: number
  weeksInUse: number
  prCount: number
}

/** Response of GET /api/home. */
export type Home = {
  currentRoutine: RoutineOverview | null
}

export function fetchHome(token: string): Promise<Home> {
  return apiRequest<Home>('/home', { token })
}
