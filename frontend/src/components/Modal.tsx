import type { ReactNode } from 'react'

type ModalProps = {
  title: string
  body?: string
  children: ReactNode
}

export function Modal({ title, body, children }: ModalProps) {
  return (
    <div className="modal-backdrop">
      <div className="card modal" role="dialog" aria-modal="true" aria-label={title}>
        <h2 className="empty-title">{title}</h2>
        {body && <p className="empty-body">{body}</p>}
        {children}
      </div>
    </div>
  )
}
