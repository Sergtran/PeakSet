export type Theme = 'system' | 'light' | 'dark'
export type WeightUnit = 'kg' | 'lb'

export type Settings = {
  prepSeconds: number
  workSeconds: number
  restSeconds: number
  sets: number
  theme: Theme
  unit: WeightUnit
}

export const defaultSettings: Settings = {
  prepSeconds: 10,
  workSeconds: 45,
  restSeconds: 60,
  sets: 3,
  theme: 'system',
  unit: 'kg',
}
