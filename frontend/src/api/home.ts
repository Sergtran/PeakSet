import { apiRequest } from './client'

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

export type Home = {
  currentRoutine: RoutineOverview | null
}

export function fetchHome(token: string): Promise<Home> {
  return apiRequest<Home>('/home', { token })
}
