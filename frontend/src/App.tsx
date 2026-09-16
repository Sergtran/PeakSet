import { useCallback, useState } from 'react'
import type { AuthResponse } from './api/auth'
import {
  clearSession,
  loadProfile,
  loadToken,
  saveSession,
  type Profile,
} from './auth/tokenStorage'
import { DashboardPage } from './pages/DashboardPage'
import { LoginPage } from './pages/LoginPage'
import './App.css'

/**
 * Root component.
 *
 * It owns the only two pieces of global state the app has right now: the JWT
 * and the signed in profile. Without a token we show the login screen,
 * otherwise we show the dashboard. Once there are more screens, this is where
 * a router will live.
 */
export default function App() {
  // Reading from localStorage in the initializer keeps the session across refreshes.
  const [token, setToken] = useState<string | null>(() => loadToken())
  const [profile, setProfile] = useState<Profile | null>(() => loadProfile())

  const handleAuthenticated = useCallback((session: AuthResponse) => {
    const nextProfile: Profile = {
      email: session.email,
      displayName: session.displayName,
    }
    saveSession(session.token, nextProfile)
    setToken(session.token)
    setProfile(nextProfile)
  }, [])

  const handleSignOut = useCallback(() => {
    clearSession()
    setToken(null)
    setProfile(null)
  }, [])

  if (!token || !profile) {
    return <LoginPage onAuthenticated={handleAuthenticated} />
  }

  return <DashboardPage token={token} profile={profile} onSignOut={handleSignOut} />
}
