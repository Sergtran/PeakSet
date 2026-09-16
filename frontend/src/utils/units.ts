import type { WeightUnit } from '../settings/types'

const poundsPerKilogram = 2.2046226218

export function fromKilograms(kilograms: number, unit: WeightUnit): number {
  const value = unit === 'lb' ? kilograms * poundsPerKilogram : kilograms
  return Math.round(value * 100) / 100
}

export function toKilograms(value: number, unit: WeightUnit): number {
  const kilograms = unit === 'lb' ? value / poundsPerKilogram : value
  return Math.round(kilograms * 100) / 100
}

export function withUnit(value: number, unit: WeightUnit): string {
  return `${value} ${unit}`
}
