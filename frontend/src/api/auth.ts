import { apiRequest } from './client'

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

export type ForgotPasswordRequest = {
  email: string
}

export type ResetPasswordRequest = {
  email: string
  token: string
  newPassword: string
}

export function login(request: LoginRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/auth/login', { method: 'POST', body: request })
}

export function register(request: RegisterRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/auth/register', { method: 'POST', body: request })
}

export function requestPasswordReset(request: ForgotPasswordRequest): Promise<void> {
  return apiRequest<void>('/auth/forgot-password', { method: 'POST', body: request })
}

export function resetPassword(request: ResetPasswordRequest): Promise<void> {
  return apiRequest<void>('/auth/reset-password', { method: 'POST', body: request })
}
