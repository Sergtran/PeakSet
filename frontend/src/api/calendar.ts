import { apiRequest } from './client'

export type CalendarWorkout = {
  id: string
  date: string
  routineName: string
  sessionName: string
}

export type CalendarDay = {
  date: string
  workouts: CalendarWorkout[]
}

export type CalendarMonth = {
  year: number
  month: number
  days: CalendarDay[]
}

export function fetchCalendarMonth(
  token: string,
  year: number,
  month: number,
): Promise<CalendarMonth> {
  return apiRequest<CalendarMonth>(`/calendar/${year}/${month}`, { token })
}
