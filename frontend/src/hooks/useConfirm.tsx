import { useCallback, useRef, useState, type ReactNode } from 'react'
import { Modal } from '../components/Modal'
import { useI18n } from '../i18n/context'

export type ConfirmRequest = {
  title: string
  body: string
  confirmLabel?: string
  danger?: boolean
}

export function useConfirm(): {
  confirm: (request: ConfirmRequest) => Promise<boolean>
  dialog: ReactNode
} {
  const { t } = useI18n()
  const [request, setRequest] = useState<ConfirmRequest | null>(null)
  const resolver = useRef<((value: boolean) => void) | null>(null)

  const confirm = useCallback((next: ConfirmRequest) => {
    setRequest(next)
    return new Promise<boolean>((resolve) => {
      resolver.current = resolve
    })
  }, [])

  function settle(value: boolean) {
    setRequest(null)
    resolver.current?.(value)
    resolver.current = null
  }

  const dialog = request ? (
    <Modal title={request.title} body={request.body}>
      <div className="button-row">
        <button className="btn btn-ghost" type="button" onClick={() => settle(false)}>
          {t('common.cancel')}
        </button>
        <button
          className={request.danger ? 'btn btn-danger' : 'btn btn-primary'}
          type="button"
          onClick={() => settle(true)}
        >
          {request.confirmLabel ?? t('common.confirm')}
        </button>
      </div>
    </Modal>
  ) : null

  return { confirm, dialog }
}
