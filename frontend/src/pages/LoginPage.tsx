import { useState, type FormEvent } from 'react'
import { login, register, type AuthResponse } from '../api/auth'
import { ApiError } from '../api/client'

type Mode = 'signin' | 'signup'

type LoginPageProps = {
  /** Called with the JWT once the API accepts the credentials. */
  onAuthenticated: (session: AuthResponse) => void
}

/**
 * Screen 1: sign in and sign up against
 * POST /api/auth/login and POST /api/auth/register.
 */
export function LoginPage({ onAuthenticated }: LoginPageProps) {
  const [mode, setMode] = useState<Mode>('signin')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [displayName, setDisplayName] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  function switchMode(next: Mode) {
    setMode(next)
    setError(null)
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      const session =
        mode === 'signin'
          ? await login({ email: email.trim(), password })
          : await register({
              email: email.trim(),
              password,
              displayName: displayName.trim() || null,
            })

      onAuthenticated(session)
    } catch (cause) {
      setError(
        cause instanceof ApiError
          ? cause.message
          : 'Something went wrong. Please try again.',
      )
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="auth-screen">
      <section className="card auth-card">
        <header className="auth-header">
          <img className="brand-mark" src="/logo-192.png" alt="" width={56} height={56} />
          <div>
            <h1 className="brand-name">PeakSet</h1>
            <p className="brand-tagline">
              Plan your training. Log every set. Track your progress.
            </p>
          </div>
        </header>

        <div className="tabs" role="tablist" aria-label="Authentication mode">
          <button
            type="button"
            role="tab"
            aria-selected={mode === 'signin'}
            className={mode === 'signin' ? 'tab tab-active' : 'tab'}
            onClick={() => switchMode('signin')}
          >
            Sign in
          </button>
          <button
            type="button"
            role="tab"
            aria-selected={mode === 'signup'}
            className={mode === 'signup' ? 'tab tab-active' : 'tab'}
            onClick={() => switchMode('signup')}
          >
            Create account
          </button>
        </div>

        <form className="form" onSubmit={handleSubmit}>
          {mode === 'signup' && (
            <label className="field">
              <span className="field-label">Name</span>
              <input
                className="field-input"
                type="text"
                name="displayName"
                autoComplete="name"
                maxLength={100}
                placeholder="How should we call you?"
                value={displayName}
                onChange={(event) => setDisplayName(event.target.value)}
              />
            </label>
          )}

          <label className="field">
            <span className="field-label">Email</span>
            <input
              className="field-input"
              type="email"
              name="email"
              autoComplete="email"
              required
              placeholder="you@example.com"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
            />
          </label>

          <label className="field">
            <span className="field-label">Password</span>
            <input
              className="field-input"
              type="password"
              name="password"
              autoComplete={mode === 'signin' ? 'current-password' : 'new-password'}
              required
              minLength={6}
              placeholder="••••••••"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
            />
            {mode === 'signup' && (
              <span className="field-hint">
                At least 6 characters, including an uppercase letter, a lowercase
                letter, a number and a symbol.
              </span>
            )}
          </label>

          {error && (
            <p className="alert alert-error" role="alert">
              {error}
            </p>
          )}

          <button className="btn btn-primary" type="submit" disabled={isSubmitting}>
            {isSubmitting
              ? 'Please wait…'
              : mode === 'signin'
                ? 'Sign in'
                : 'Create account'}
          </button>
        </form>
      </section>
    </main>
  )
}
