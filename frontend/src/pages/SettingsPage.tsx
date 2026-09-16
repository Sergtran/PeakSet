import { ErrorList, LoadingState } from '../components/Feedback'
import { ScreenHeader } from '../components/ScreenHeader'
import { useI18n } from '../i18n/context'
import { useSettings } from '../settings/context'
import type { Theme, WeightUnit } from '../settings/types'

export function SettingsPage() {
  const { t } = useI18n()
  const { settings, update, isLoading, error } = useSettings()

  if (isLoading) {
    return (
      <>
        <ScreenHeader title={t('settings.title')} />
        <LoadingState />
      </>
    )
  }

  return (
    <>
      <ScreenHeader title={t('settings.title')} />

      {error !== null && <ErrorList error={error} />}

      <section className="card settings-group">
        <h2 className="empty-title">{t('settings.timer')}</h2>
        <div className="field-row">
          <NumberField
            label={t('settings.prep')}
            value={settings.prepSeconds}
            min={1}
            onChange={(value) => update({ prepSeconds: value })}
          />
          <NumberField
            label={t('settings.work')}
            value={settings.workSeconds}
            min={1}
            onChange={(value) => update({ workSeconds: value })}
          />
          <NumberField
            label={t('settings.rest')}
            value={settings.restSeconds}
            min={1}
            onChange={(value) => update({ restSeconds: value })}
          />
          <NumberField
            label={t('settings.sets')}
            value={settings.sets}
            min={1}
            onChange={(value) => update({ sets: value })}
          />
        </div>
      </section>

      <section className="card settings-group">
        <h2 className="empty-title">{t('settings.units')}</h2>
        <select
          className="field-input"
          value={settings.unit}
          onChange={(event) => update({ unit: event.target.value as WeightUnit })}
        >
          <option value="kg">{t('settings.unitsKg')}</option>
          <option value="lb">{t('settings.unitsLb')}</option>
        </select>
        <p className="list-row-meta">{t('settings.unitsNote')}</p>
      </section>

      <section className="card settings-group">
        <h2 className="empty-title">{t('settings.theme')}</h2>
        <select
          className="field-input"
          value={settings.theme}
          onChange={(event) => update({ theme: event.target.value as Theme })}
        >
          <option value="system">{t('settings.themeSystem')}</option>
          <option value="light">{t('settings.themeLight')}</option>
          <option value="dark">{t('settings.themeDark')}</option>
        </select>
      </section>
    </>
  )
}

function NumberField({
  label,
  value,
  min,
  onChange,
}: {
  label: string
  value: number
  min: number
  onChange: (value: number) => void
}) {
  return (
    <label className="field">
      <span className="field-label">{label}</span>
      <input
        className="field-input"
        type="number"
        min={min}
        max={3600}
        value={value}
        onChange={(event) => onChange(Math.max(min, Number(event.target.value) || min))}
      />
    </label>
  )
}
