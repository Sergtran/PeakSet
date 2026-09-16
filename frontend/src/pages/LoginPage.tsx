import { useState, type FormEvent } from 'react'
import {
  login,
  register,
  requestPasswordReset,
  resetPassword,
  type AuthResponse,
} from '../api/auth'
import { LanguageSwitcher } from '../components/LanguageSwitcher'
import { describeApiError } from '../i18n/apiErrors'
import { useI18n } from '../i18n/context'

type Mode = 'signin' | 'signup' | 'reset'

type LoginPageProps = {
  onAuthenticated: (session: AuthResponse) => void
}

export function LoginPage({ onAuthenticated }: LoginPageProps) {
  const { t } = useI18n()
  const [mode, setMode] = useState<Mode>('signin')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [displayName, setDisplayName] = useState('')
  const [resetToken, setResetToken] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [resetRequested, setResetRequested] = useState(false)
  const [failure, setFailure] = useState<unknown | null>(null)
  const [message, setMessage] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const errors = failure !== null ? describeApiError(failure, t) : []

  function switchMode(next: Mode) {
    setMode(next)
    setFailure(null)
    setMessage(null)
    setResetRequested(false)
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setFailure(null)
    setMessage(null)
    setIsSubmitting(true)

    try {
      if (mode === 'signin') {
        onAuthenticated(await login({ email: email.trim(), password }))
        return
      }

      if (mode === 'signup') {
        onAuthenticated(
          await register({
            email: email.trim(),
            password,
            displayName: displayName.trim() || null,
          }),
        )
        return
      }

      if (!resetRequested) {
        await requestPasswordReset({ email: email.trim() })
        setResetRequested(true)
        return
      }

      await resetPassword({
        email: email.trim(),
        token: resetToken.trim(),
        newPassword,
      })
      switchMode('signin')
      setPassword('')
      setMessage(t('auth.passwordChanged'))
    } catch (cause) {
      setFailure(cause)
    } finally {
      setIsSubmitting(false)
    }
  }

  const submitLabel = isSubmitting
    ? t('auth.pleaseWait')
    : mode === 'signin'
      ? t('auth.signIn')
      : mode === 'signup'
        ? t('auth.createAccount')
        : resetRequested
          ? t('auth.changePassword')
          : t('auth.sendResetLink')

  return (
    <main className="auth-screen">
      <section className="card auth-card">
        <header className="auth-header">
          <img className="brand-mark" src="/logo-192.png" alt="" width={56} height={56} />
          <div className="auth-header-text">
            <h1 className="brand-name">{t('app.name')}</h1>
            <p className="brand-tagline">{t('app.tagline')}</p>
          </div>
          <LanguageSwitcher />
        </header>

        {mode === 'reset' ? (
          <div className="reset-header">
            <h2 className="empty-title">{t('auth.resetTitle')}</h2>
            <button type="button" className="link-button" onClick={() => switchMode('signin')}>
              {t('auth.backToSignIn')}
            </button>
          </div>
        ) : (
          <div className="tabs" role="tablist" aria-label="Authentication mode">
            <button
              type="button"
              role="tab"
              aria-selected={mode === 'signin'}
              className={mode === 'signin' ? 'tab tab-active' : 'tab'}
              onClick={() => switchMode('signin')}
            >
              {t('auth.tabSignIn')}
            </button>
            <button
              type="button"
              role="tab"
              aria-selected={mode === 'signup'}
              className={mode === 'signup' ? 'tab tab-active' : 'tab'}
              onClick={() => switchMode('signup')}
            >
              {t('auth.tabSignUp')}
            </button>
          </div>
        )}

        <form className="form" onSubmit={handleSubmit}>
          {mode === 'signup' && (
            <label className="field">
              <span className="field-label">{t('auth.name')}</span>
              <input
                className="field-input"
                type="text"
                name="displayName"
                autoComplete="name"
                maxLength={100}
                placeholder={t('auth.namePlaceholder')}
                value={displayName}
                onChange={(event) => setDisplayName(event.target.value)}
              />
            </label>
          )}

          <label className="field">
            <span className="field-label">{t('auth.email')}</span>
            <input
              className="field-input"
              type="email"
              name="email"
              autoComplete="email"
              required
              placeholder={t('auth.emailPlaceholder')}
              value={email}
              onChange={(event) => setEmail(event.target.value)}
            />
          </label>

          {mode !== 'reset' && (
            <label className="field">
              <span className="field-label">{t('auth.password')}</span>
              <input
                className="field-input"
                type="password"
                name="password"
                autoComplete={mode === 'signin' ? 'current-password' : 'new-password'}
                required
                minLength={6}
                value={password}
                onChange={(event) => setPassword(event.target.value)}
              />
              {mode === 'signup' && <span className="field-hint">{t('auth.passwordHint')}</span>}
            </label>
          )}

          {mode === 'reset' && resetRequested && (
            <>
              <p className="alert alert-ok">{t('auth.resetSent')}</p>

              <label className="field">
                <span className="field-label">{t('auth.resetToken')}</span>
                <input
                  className="field-input"
                  name="resetToken"
                  required
                  placeholder={t('auth.resetTokenPlaceholder')}
                  value={resetToken}
                  onChange={(event) => setResetToken(event.target.value)}
                />
              </label>

              <label className="field">
                <span className="field-label">{t('auth.newPassword')}</span>
                <input
                  className="field-input"
                  type="password"
                  name="newPassword"
                  autoComplete="new-password"
                  required
                  minLength={6}
                  value={newPassword}
                  onChange={(event) => setNewPassword(event.target.value)}
                />
                <span className="field-hint">{t('auth.passwordHint')}</span>
              </label>
            </>
          )}

          {message !== null && <p className="alert alert-ok">{message}</p>}

          {errors.length > 0 && (
            <ul className="alert alert-error" role="alert">
              {errors.map((text) => (
                <li key={text}>{text}</li>
              ))}
            </ul>
          )}

          <button className="btn btn-primary" type="submit" disabled={isSubmitting}>
            {submitLabel}
          </button>

          {mode === 'signin' && (
            <button type="button" className="link-button" onClick={() => switchMode('reset')}>
              {t('auth.forgotPassword')}
            </button>
          )}
        </form>
      </section>
    </main>
  )
}
