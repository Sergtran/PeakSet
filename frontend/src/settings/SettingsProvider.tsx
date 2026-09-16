import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from 'react'
import { fetchSettings, saveSettings, type UserSettings } from '../api/settings'
import { useAuth } from '../auth/context'
import { SettingsContext, type SettingsValue } from './context'
import { applyTheme } from './theme'
import { defaultSettings, type Settings, type Theme, type WeightUnit } from './types'

const unitKey = 'peakset.unit'
const saveDelayMs = 600

function readUnit(): WeightUnit {
  try {
    return localStorage.getItem(unitKey) === 'lb' ? 'lb' : 'kg'
  } catch {
    return 'kg'
  }
}

function writeUnit(unit: WeightUnit): void {
  try {
    localStorage.setItem(unitKey, unit)
  } catch {
    return
  }
}

function fromApi(dto: UserSettings, unit: WeightUnit): Settings {
  return {
    prepSeconds: dto.timerPrepSeconds,
    workSeconds: dto.timerWorkSeconds,
    restSeconds: dto.timerRestSeconds,
    sets: dto.timerSets,
    theme: dto.theme.toLowerCase() as Theme,
    unit,
  }
}

export function SettingsProvider({ children }: { children: ReactNode }) {
  const { token } = useAuth()
  const [settings, setSettings] = useState<Settings>(() => ({
    ...defaultSettings,
    unit: readUnit(),
  }))
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<unknown>(null)
  const saveTimer = useRef<number | null>(null)

  useEffect(() => {
    let isActive = true

    fetchSettings(token)
      .then((dto) => {
        if (!isActive) return
        const next = fromApi(dto, readUnit())
        setSettings(next)
        applyTheme(next.theme)
        setError(null)
      })
      .catch((cause: unknown) => {
        if (isActive) setError(cause)
      })
      .finally(() => {
        if (isActive) setIsLoading(false)
      })

    return () => {
      isActive = false
    }
  }, [token])

  useEffect(
    () => () => {
      if (saveTimer.current !== null) {
        window.clearTimeout(saveTimer.current)
      }
    },
    [],
  )

  const update = useCallback(
    (patch: Partial<Settings>) => {
      setSettings((current) => {
        const next = { ...current, ...patch }

        if (patch.unit) {
          writeUnit(patch.unit)
        }
        if (patch.theme) {
          applyTheme(patch.theme)
        }

        if (saveTimer.current !== null) {
          window.clearTimeout(saveTimer.current)
        }
        saveTimer.current = window.setTimeout(() => {
          void saveSettings(token, {
            theme: (next.theme.charAt(0).toUpperCase() + next.theme.slice(1)) as UserSettings['theme'],
            timerPrepSeconds: next.prepSeconds,
            timerWorkSeconds: next.workSeconds,
            timerRestSeconds: next.restSeconds,
            timerSets: next.sets,
          }).catch((cause: unknown) => setError(cause))
        }, saveDelayMs)

        return next
      })
    },
    [token],
  )

  const value = useMemo<SettingsValue>(
    () => ({ settings, isLoading, error, update }),
    [settings, isLoading, error, update],
  )

  return <SettingsContext.Provider value={value}>{children}</SettingsContext.Provider>
}
