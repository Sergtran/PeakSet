import { createContext, useContext } from 'react'
import type { Language, TranslationKey } from './translations'

export type TranslateParams = Record<string, string | number>
export type Translate = (key: TranslationKey, params?: TranslateParams) => string

export type I18nValue = {
  language: Language
  setLanguage: (language: Language) => void
  t: Translate
}

export const I18nContext = createContext<I18nValue | null>(null)

export function useI18n(): I18nValue {
  const value = useContext(I18nContext)
  if (!value) {
    throw new Error('useI18n must be used inside I18nProvider')
  }
  return value
}
