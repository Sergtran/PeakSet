import type { ReactNode } from 'react'
import { describeApiError } from '../i18n/apiErrors'
import { useI18n } from '../i18n/context'

export function ErrorList({ error }: { error: unknown }) {
  const { t } = useI18n()
  const messages = describeApiError(error, t)

  return (
    <ul className="alert alert-error" role="alert">
      {messages.map((message) => (
        <li key={message}>{message}</li>
      ))}
    </ul>
  )
}

export function LoadingState({ label }: { label?: string }) {
  const { t } = useI18n()
  return <p className="state-message">{label ?? t('common.loading')}</p>
}

export function EmptyState({
  title,
  body,
  children,
}: {
  title: string
  body: string
  children?: ReactNode
}) {
  return (
    <section className="card empty-state">
      <h2 className="empty-title">{title}</h2>
      <p className="empty-body">{body}</p>
      {children}
    </section>
  )
}
