/**
 * Keeps the session (JWT + basic profile) in localStorage so the user is still
 * signed in after a page refresh.
 *
 * Note: localStorage is readable by any script on the page, so this is only
 * good enough for a training app. If PeakSet ever handles sensitive data we
 * should move the token into an httpOnly cookie.
 */

const TOKEN_KEY = 'peakset.token'
const PROFILE_KEY = 'peakset.profile'

export type Profile = {
  email: string
  displayName: string | null
}

export function saveSession(token: string, profile: Profile): void {
  try {
    localStorage.setItem(TOKEN_KEY, token)
    localStorage.setItem(PROFILE_KEY, JSON.stringify(profile))
  } catch {
    // Private browsing modes can block storage. Staying signed in is not
    // critical, so we simply keep the session in memory.
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
    // Nothing to do: the in-memory session is cleared by the caller anyway.
  }
}
