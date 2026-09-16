import { useState, type ChangeEvent } from 'react'
import { exportData, importData, type PeakSetExport } from '../api/data'
import { useAuth } from '../auth/context'
import { ErrorList } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useConfirm } from '../hooks/useConfirm'
import { useI18n } from '../i18n/context'
import { todayKey } from '../utils/format'

export function DataPage() {
  const { t } = useI18n()
  const { token } = useAuth()
  const { confirm, dialog } = useConfirm()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<unknown>(null)
  const [status, setStatus] = useState<string | null>(null)

  async function handleExport() {
    setBusy(true)
    setError(null)
    setStatus(null)
    try {
      const snapshot = await exportData(token)
      const blob = new Blob([JSON.stringify(snapshot, null, 2)], { type: 'application/json' })
      const url = URL.createObjectURL(blob)
      const link = document.createElement('a')
      link.href = url
      link.download = `peakset-${todayKey()}.json`
      link.click()
      URL.revokeObjectURL(url)
      setStatus(t('data.exported'))
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
    }
  }

  async function handleFile(event: ChangeEvent<HTMLInputElement>) {
    const file = event.target.files?.[0]
    if (!file) return
    setError(null)
    setStatus(null)

    const accepted = await confirm({
      title: t('data.import'),
      body: t('data.confirmImport'),
      confirmLabel: t('data.import'),
      danger: true,
    })
    if (!accepted) {
      event.target.value = ''
      return
    }

    setBusy(true)
    try {
      const text = await file.text()
      const snapshot = JSON.parse(text) as PeakSetExport
      await importData(token, snapshot)
      setStatus(t('data.imported'))
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
      event.target.value = ''
    }
  }

  return (
    <>
      <ScreenHeader title={t('menu.data')} />

      {error !== null && <ErrorList error={error} />}
      {status !== null && <p className="alert alert-ok">{status}</p>}

      <section className="card settings-group">
        <h2 className="empty-title">{t('data.export')}</h2>
        <p className="list-row-meta">{t('data.exportHint')}</p>
        <button className="btn btn-primary" type="button" disabled={busy} onClick={() => void handleExport()}>
          {t('data.export')}
        </button>
      </section>

      <section className="card settings-group">
        <h2 className="empty-title">{t('data.import')}</h2>
        <p className="list-row-meta">{t('data.importHint')}</p>
        <p className="list-row-meta">{t('data.importWarning')}</p>
        <input
          className="field-input"
          type="file"
          accept="application/json,.json"
          disabled={busy}
          onChange={(event) => void handleFile(event)}
        />
      </section>
      {dialog}
    </>
  )
}
