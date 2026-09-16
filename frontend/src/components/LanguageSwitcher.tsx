import { useI18n } from '../i18n/context'
import { languages, type Language, type TranslationKey } from '../i18n/translations'

const labelKeys: Record<Language, TranslationKey> = {
  en: 'language.en',
  es: 'language.es',
}

export function LanguageSwitcher() {
  const { language, setLanguage, t } = useI18n()

  return (
    <select
      className="language-select"
      aria-label={t('language.label')}
      value={language}
      onChange={(event) => setLanguage(event.target.value as Language)}
    >
      {languages.map((code) => (
        <option key={code} value={code}>
          {t(labelKeys[code])}
        </option>
      ))}
    </select>
  )
}
