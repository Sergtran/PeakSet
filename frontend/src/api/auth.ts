import { apiRequest } from './client'

/** What the API returns from /api/auth/login and /api/auth/register. */
export type AuthResponse = {
  token: string
  email: string
  displayName: string | null
}

export type LoginRequest = {
  email: string
  password: string
}

export type RegisterRequest = LoginRequest & {
  displayName: string | null
}

export function login(request: LoginRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/auth/login', { method: 'POST', body: request })
}

export function register(request: RegisterRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/auth/register', { method: 'POST', body: request })
}
