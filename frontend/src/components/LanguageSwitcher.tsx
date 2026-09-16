import { Languages } from 'lucide-react'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { useI18n } from '@/i18n/context'
import type { Language } from '@/i18n/translations'

const names: Record<Language, string> = {
  en: 'English',
  es: 'Español',
}

export function LanguageSwitcher({ className }: { className?: string }) {
  const { language, setLanguage, t } = useI18n()

  return (
    <Select
      value={language}
      onValueChange={(next) => setLanguage(next as Language)}
    >
      <SelectTrigger className={className} aria-label={t('language.label')}>
        <Languages className="size-4 text-muted-foreground" />
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        {(Object.keys(names) as Language[]).map((code) => (
          <SelectItem key={code} value={code}>
            {names[code]}
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  )
}
