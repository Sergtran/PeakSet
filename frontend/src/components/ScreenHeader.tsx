import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'

type ScreenHeaderProps = {
  title: string
  subtitle?: string
  actions?: React.ReactNode
}

export function ScreenHeader({ title, subtitle, actions }: ScreenHeaderProps) {
  const { t } = useI18n()
  const { back, canGoBack } = useNavigation()

  return (
    <header className="screen-header">
      {canGoBack && (
        <button className="btn btn-ghost screen-back" type="button" onClick={back}>
          ← {t('common.back')}
        </button>
      )}
      <div className="screen-header-text">
        <h1 className="page-title">{title}</h1>
        {subtitle && <p className="page-subtitle">{subtitle}</p>}
      </div>
      {actions}
    </header>
  )
}
