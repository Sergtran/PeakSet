import { type ReactNode } from 'react'
import { AuthContext } from './context'
import type { Profile } from './tokenStorage'

type AuthProviderProps = {
  token: string
  profile: Profile
  signOut: () => void
  children: ReactNode
}

export function AuthProvider({ token, profile, signOut, children }: AuthProviderProps) {
  return <AuthContext.Provider value={{ token, profile, signOut }}>{children}</AuthContext.Provider>
}
