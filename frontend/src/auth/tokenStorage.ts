const TOKEN_KEY = 'liftraza.token'
const PROFILE_KEY = 'liftraza.profile'
const LANGUAGE_KEY = 'liftraza.language'

export type Profile = {
  email: string
  displayName: string | null
}

export function saveSession(token: string, profile: Profile): void {
  try {
    localStorage.setItem(TOKEN_KEY, token)
    localStorage.setItem(PROFILE_KEY, JSON.stringify(profile))
  } catch {
    return
  }
}

export function loadToken(): string | null {
  try {
    return localStorage.getItem(TOKEN_KEY)
  } catch {
    return null
  }
}

export function loadProfile(): Profile | null {
  try {
    const raw = localStorage.getItem(PROFILE_KEY)
    if (!raw) {
      return null
    }
    const parsed = JSON.parse(raw) as Profile
    return typeof parsed?.email === 'string' ? parsed : null
  } catch {
    return null
  }
}

export function clearSession(): void {
  try {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(PROFILE_KEY)
  } catch {
    return
  }
}

export function loadStoredLanguage(): string | null {
  try {
    return localStorage.getItem(LANGUAGE_KEY)
  } catch {
    return null
  }
}

export function saveStoredLanguage(language: string): void {
  try {
    localStorage.setItem(LANGUAGE_KEY, language)
  } catch {
    return
  }
}
