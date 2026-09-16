import { ChevronLeft } from 'lucide-react'
import type { ReactNode } from 'react'
import { Button } from '@/components/ui/button'
import { useI18n } from '@/i18n/context'
import { useNavigation } from '@/navigation/context'

type PageHeaderProps = {
  title: string
  subtitle?: string
  onBack?: () => void
  actions?: ReactNode
  children?: ReactNode
}

export function PageHeader({ title, subtitle, onBack, actions, children }: PageHeaderProps) {
  const { t } = useI18n()
  const { back, canGoBack } = useNavigation()
  const goBack = onBack ?? back

  return (
    <div className="grid gap-4">
      <header className="grid gap-2">
        {(canGoBack || onBack) && (
          <Button variant="ghost" size="sm" className="w-fit -ml-2" onClick={goBack}>
            <ChevronLeft className="size-4" />
            {t('common.back')}
          </Button>
        )}
        <div className="flex items-start gap-3">
          <div className="min-w-0 flex-1">
            <h1 className="truncate text-xl font-semibold tracking-tight">{title}</h1>
            {subtitle && <p className="truncate text-sm text-muted-foreground">{subtitle}</p>}
          </div>
          {actions}
        </div>
      </header>
      {children}
    </div>
  )
}
