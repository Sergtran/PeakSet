import type { Theme } from './types'

const themeCacheKey = 'peakset.theme'

function resolveTheme(theme: Theme): 'light' | 'dark' {
  if (theme !== 'system') {
    return theme
  }
  return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
}

export function applyTheme(theme: Theme): void {
  const resolved = resolveTheme(theme)
  document.documentElement.classList.toggle('dark', resolved === 'dark')
  try {
    localStorage.setItem(themeCacheKey, resolved)
  } catch {
    return
  }
}

// Runs before React mounts, so the first paint already has the right theme.
export function applyCachedTheme(): void {
  try {
    const cached = localStorage.getItem(themeCacheKey)
    if (cached === 'light' || cached === 'dark') {
      document.documentElement.classList.toggle('dark', cached === 'dark')
      return
    }
    document.documentElement.classList.toggle(
      'dark',
      window.matchMedia('(prefers-color-scheme: dark)').matches,
    )
  } catch {
    return
  }
}
