import { useState, type FormEvent } from 'react'
import { useI18n } from '../i18n/context'

type InlineNameFormProps = {
  initialValue?: string
  placeholder: string
  submitLabel: string
  disabled?: boolean
  autoFocus?: boolean
  onSubmit: (value: string) => void
  onCancel?: () => void
}

export function InlineNameForm({
  initialValue = '',
  placeholder,
  submitLabel,
  disabled = false,
  autoFocus = false,
  onSubmit,
  onCancel,
}: InlineNameFormProps) {
  const { t } = useI18n()
  const [value, setValue] = useState(initialValue)
  const trimmed = value.trim()

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!trimmed) return
    onSubmit(trimmed)
    setValue(initialValue)
  }

  return (
    <form className="inline-form" onSubmit={handleSubmit}>
      <input
        className="field-input"
        value={value}
        placeholder={placeholder}
        autoFocus={autoFocus}
        maxLength={150}
        onChange={(event) => setValue(event.target.value)}
      />
      <button className="btn btn-primary" type="submit" disabled={disabled || !trimmed}>
        {submitLabel}
      </button>
      {onCancel && (
        <button className="btn btn-ghost" type="button" onClick={onCancel}>
          {t('common.cancel')}
        </button>
      )}
    </form>
  )
}
