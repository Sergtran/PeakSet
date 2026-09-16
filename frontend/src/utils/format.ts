export function formatDateTime(value: string, language: string): string {
  return new Intl.DateTimeFormat(language, { dateStyle: 'medium', timeStyle: 'short' }).format(
    new Date(value),
  )
}

export function formatDay(value: string, language: string): string {
  return new Intl.DateTimeFormat(language, { dateStyle: 'medium' }).format(new Date(value))
}

export function formatMonth(year: number, month: number, language: string): string {
  return new Intl.DateTimeFormat(language, { month: 'long', year: 'numeric' }).format(
    new Date(year, month - 1, 1),
  )
}

export function dateKey(year: number, month: number, day: number): string {
  return `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`
}

export function todayKey(): string {
  const now = new Date()
  return dateKey(now.getFullYear(), now.getMonth() + 1, now.getDate())
}

export function startOfMonthKey(value: string): string {
  return value.slice(0, 10)
}
