import { useCallback, useState } from 'react'
import './App.css'
import type { AuthResponse } from './api/auth'
import { AuthProvider } from './auth/AuthProvider'
import {
  clearSession,
  loadProfile,
  loadToken,
  saveSession,
  type Profile,
} from './auth/tokenStorage'
import { AppShell } from './components/AppShell'
import { NavigationProvider } from './navigation/NavigationProvider'
import { LoginPage } from './pages/LoginPage'
import { SettingsProvider } from './settings/SettingsProvider'

export default function App() {
  const [token, setToken] = useState<string | null>(() => loadToken())
  const [profile, setProfile] = useState<Profile | null>(() => loadProfile())

  const handleAuthenticated = useCallback((session: AuthResponse) => {
    const nextProfile: Profile = { email: session.email, displayName: session.displayName }
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

  return (
    <AuthProvider token={token} profile={profile} signOut={handleSignOut}>
      <SettingsProvider>
        <NavigationProvider>
          <AppShell />
        </NavigationProvider>
      </SettingsProvider>
    </AuthProvider>
  )
}
