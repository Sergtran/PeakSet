import { useRef, useState, type ChangeEvent } from 'react'
import { Download, LogOut, Upload, User } from 'lucide-react'
import { exportData, importData, type LiftrazaExport } from '@/api/data'
import { useAuth } from '@/auth/context'
import { LanguageSwitcher } from '@/components/LanguageSwitcher'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { useConfirm } from '@/hooks/useConfirm'
import { useI18n } from '@/i18n/context'
import { useSettings } from '@/settings/context'
import type { Theme, WeightUnit } from '@/settings/types'
import { todayKey } from '@/utils/format'

export function SettingsPage() {
  const { t } = useI18n()
  const { profile, token, signOut } = useAuth()
  const { settings, update } = useSettings()
  const { confirm, dialog } = useConfirm()
  const [busy, setBusy] = useState(false)
  const [notice, setNotice] = useState<string | null>(null)
  const [error, setError] = useState<unknown>(null)
  const fileInput = useRef<HTMLInputElement>(null)

  async function handleExport() {
    setBusy(true)
    setError(null)
    setNotice(null)
    try {
      const snapshot = await exportData(token)
      const blob = new Blob([JSON.stringify(snapshot, null, 2)], { type: 'application/json' })
      const url = URL.createObjectURL(blob)
      const link = document.createElement('a')
      link.href = url
      link.download = `liftraza-${todayKey()}.json`
      link.click()
      URL.revokeObjectURL(url)
      setNotice(t('data.exported'))
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
    }
  }

  async function handleImport(event: ChangeEvent<HTMLInputElement>) {
    const file = event.target.files?.[0]
    if (!file) return

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
    setError(null)
    setNotice(null)
    try {
      const snapshot = JSON.parse(await file.text()) as LiftrazaExport
      await importData(token, snapshot)
      setNotice(t('data.imported'))
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
      event.target.value = ''
    }
  }

  return (
    <div className="grid gap-5">
      <h1 className="text-2xl font-semibold tracking-tight">{t('settings.title')}</h1>

      {notice !== null && (
        <p className="rounded-md border border-primary/30 bg-accent px-3 py-2 text-sm text-accent-foreground">
          {notice}
        </p>
      )}
      {error !== null && (
        <p className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive">
          {t('settings.saveFailed')}
        </p>
      )}

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{t('settings.profile')}</CardTitle>
          <CardDescription>{t('settings.profileNote')}</CardDescription>
        </CardHeader>
        <CardContent className="flex items-center gap-3">
          <span className="grid size-11 place-items-center rounded-full bg-accent text-accent-foreground">
            <User className="size-5" />
          </span>
          <div className="min-w-0">
            <p className="truncate font-medium">{profile.displayName || profile.email}</p>
            <p className="truncate text-sm text-muted-foreground">{profile.email}</p>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{t('settings.preferences')}</CardTitle>
        </CardHeader>
        <CardContent className="grid gap-4">
          <div className="grid gap-2">
            <Label>{t('language.label')}</Label>
            <LanguageSwitcher className="w-full" />
          </div>

          <div className="grid gap-2">
            <Label>{t('settings.theme')}</Label>
            <Select
              value={settings.theme}
              onValueChange={(value) => update({ theme: value as Theme })}
            >
              <SelectTrigger className="w-full">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="system">{t('settings.themeSystem')}</SelectItem>
                <SelectItem value="light">{t('settings.themeLight')}</SelectItem>
                <SelectItem value="dark">{t('settings.themeDark')}</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-2">
            <Label>{t('settings.units')}</Label>
            <Select
              value={settings.unit}
              onValueChange={(value) => update({ unit: value as WeightUnit })}
            >
              <SelectTrigger className="w-full">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="kg">{t('settings.unitsKg')}</SelectItem>
                <SelectItem value="lb">{t('settings.unitsLb')}</SelectItem>
              </SelectContent>
            </Select>
            <p className="text-xs text-muted-foreground">{t('settings.unitsNote')}</p>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{t('settings.timer')}</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-2 gap-4">
          <NumberField
            id="prep"
            label={t('settings.prep')}
            value={settings.prepSeconds}
            min={1}
            onChange={(value) => update({ prepSeconds: value })}
          />
          <NumberField
            id="work"
            label={t('settings.work')}
            value={settings.workSeconds}
            min={1}
            onChange={(value) => update({ workSeconds: value })}
          />
          <NumberField
            id="rest"
            label={t('settings.rest')}
            value={settings.restSeconds}
            min={1}
            onChange={(value) => update({ restSeconds: value })}
          />
          <NumberField
            id="sets"
            label={t('settings.sets')}
            value={settings.sets}
            min={1}
            onChange={(value) => update({ sets: value })}
          />
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">{t('menu.data')}</CardTitle>
          <CardDescription>{t('data.exportHint')}</CardDescription>
        </CardHeader>
        <CardContent className="grid gap-3">
          <Button variant="outline" disabled={busy} onClick={() => void handleExport()}>
            <Download className="size-4" />
            {t('data.export')}
          </Button>

          <Button
            variant="outline"
            disabled={busy}
            onClick={() => fileInput.current?.click()}
          >
            <Upload className="size-4" />
            {t('data.import')}
          </Button>
          <input
            ref={fileInput}
            type="file"
            accept="application/json,.json"
            className="hidden"
            onChange={(event) => void handleImport(event)}
          />
          <p className="text-xs text-muted-foreground">{t('data.importWarning')}</p>
        </CardContent>
      </Card>

      <Card>
        <CardContent className="pt-6">
          <Separator className="mb-4" />
          <Button variant="outline" className="w-full" onClick={signOut}>
            <LogOut className="size-4" />
            {t('menu.signOut')}
          </Button>
        </CardContent>
      </Card>

      {dialog}
    </div>
  )
}

type NumberFieldProps = {
  id: string
  label: string
  value: number
  min: number
  onChange: (value: number) => void
}

function NumberField({ id, label, value, min, onChange }: NumberFieldProps) {
  return (
    <div className="grid gap-2">
      <Label htmlFor={id}>{label}</Label>
      <Input
        id={id}
        type="number"
        inputMode="numeric"
        min={min}
        max={3600}
        value={value}
        onChange={(event) => onChange(Math.max(min, Number(event.target.value) || min))}
      />
    </div>
  )
}
