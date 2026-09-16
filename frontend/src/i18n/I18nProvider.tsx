import { useCallback, useMemo, useState, type ReactNode } from 'react'
import { loadStoredLanguage, saveStoredLanguage } from '../auth/tokenStorage'
import { I18nContext, type TranslateParams, type I18nValue } from './context'
import { dictionaries, languages, type Language } from './translations'

function detectLanguage(): Language {
  const stored = loadStoredLanguage()
  if (stored && languages.includes(stored as Language)) {
    return stored as Language
  }
  return navigator.language.toLowerCase().startsWith('es') ? 'es' : 'en'
}

function interpolate(text: string, params?: TranslateParams): string {
  if (!params) {
    return text
  }
  return text.replace(/\{(\w+)\}/g, (match, name: string) =>
    name in params ? String(params[name]) : match,
  )
}

export function I18nProvider({ children }: { children: ReactNode }) {
  const [language, setLanguageState] = useState<Language>(detectLanguage)

  const setLanguage = useCallback((next: Language) => {
    saveStoredLanguage(next)
    setLanguageState(next)
  }, [])

  const value = useMemo<I18nValue>(() => {
    const dictionary = dictionaries[language]
    return {
      language,
      setLanguage,
      t: (key, params) => interpolate(dictionary[key], params),
    }
  }, [language, setLanguage])

  return <I18nContext.Provider value={value}>{children}</I18nContext.Provider>
}
