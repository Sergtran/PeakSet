import { useState, type FormEvent } from 'react'
import { Loader2 } from 'lucide-react'
import {
  login,
  register,
  requestPasswordReset,
  resetPassword,
  type AuthResponse,
} from '@/api/auth'
import { LanguageSwitcher } from '@/components/LanguageSwitcher'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { describeApiError } from '@/i18n/apiErrors'
import { useI18n } from '@/i18n/context'

type Mode = 'signin' | 'signup'

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
  const [isResetting, setIsResetting] = useState(false)
  const [resetRequested, setResetRequested] = useState(false)
  const [failure, setFailure] = useState<unknown | null>(null)
  const [message, setMessage] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const errors = failure !== null ? describeApiError(failure, t) : []

  function changeMode(next: Mode) {
    setMode(next)
    setFailure(null)
    setMessage(null)
    setIsResetting(false)
    setResetRequested(false)
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setFailure(null)
    setMessage(null)
    setIsSubmitting(true)

    try {
      if (isResetting && !resetRequested) {
        await requestPasswordReset({ email: email.trim() })
        setResetRequested(true)
        return
      }

      if (isResetting) {
        await resetPassword({ email: email.trim(), token: resetToken.trim(), newPassword })
        setIsResetting(false)
        setResetRequested(false)
        setPassword('')
        setResetToken('')
        setNewPassword('')
        setMessage(t('auth.passwordChanged'))
        return
      }

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
      setFailure(cause)
    } finally {
      setIsSubmitting(false)
    }
  }

  const submitLabel = isSubmitting
    ? t('auth.pleaseWait')
    : isResetting
      ? resetRequested
        ? t('auth.changePassword')
        : t('auth.sendResetLink')
      : mode === 'signin'
        ? t('auth.signIn')
        : t('auth.createAccount')

  return (
    <div className="grid min-h-svh place-items-center bg-background p-5">
      <Card className="w-full max-w-md">
        <CardHeader>
          <div className="mb-2 flex items-center justify-between gap-3">
            <img
              src="/logo-192.png"
              alt=""
              width={48}
              height={48}
              className="size-12 rounded-xl"
            />
            <LanguageSwitcher className="w-auto" />
          </div>
          <CardTitle className="text-2xl">{t('app.name')}</CardTitle>
          <CardDescription>{t('app.tagline')}</CardDescription>
        </CardHeader>

        <CardContent className="grid gap-5">
          {!isResetting && (
            <Tabs value={mode} onValueChange={(value) => changeMode(value as Mode)}>
              <TabsList className="grid w-full grid-cols-2">
                <TabsTrigger value="signin">{t('auth.tabSignIn')}</TabsTrigger>
                <TabsTrigger value="signup">{t('auth.tabSignUp')}</TabsTrigger>
              </TabsList>
              <TabsContent value="signin" />
              <TabsContent value="signup" />
            </Tabs>
          )}

          {isResetting && (
            <div className="grid gap-1">
              <h2 className="text-lg font-semibold">{t('auth.resetTitle')}</h2>
              <Button
                variant="link"
                className="h-auto w-fit p-0"
                onClick={() => {
                  setIsResetting(false)
                  setResetRequested(false)
                  setFailure(null)
                }}
              >
                {t('auth.backToSignIn')}
              </Button>
            </div>
          )}

          <form className="grid gap-4" onSubmit={handleSubmit}>
            {mode === 'signup' && !isResetting && (
              <div className="grid gap-2">
                <Label htmlFor="displayName">{t('auth.name')}</Label>
                <Input
                  id="displayName"
                  autoComplete="name"
                  maxLength={100}
                  placeholder={t('auth.namePlaceholder')}
                  value={displayName}
                  onChange={(event) => setDisplayName(event.target.value)}
                />
              </div>
            )}

            <div className="grid gap-2">
              <Label htmlFor="email">{t('auth.email')}</Label>
              <Input
                id="email"
                type="email"
                autoComplete="email"
                required
                placeholder={t('auth.emailPlaceholder')}
                value={email}
                onChange={(event) => setEmail(event.target.value)}
              />
            </div>

            {!isResetting && (
              <div className="grid gap-2">
                <Label htmlFor="password">{t('auth.password')}</Label>
                <Input
                  id="password"
                  type="password"
                  autoComplete={mode === 'signin' ? 'current-password' : 'new-password'}
                  required
                  minLength={6}
                  value={password}
                  onChange={(event) => setPassword(event.target.value)}
                />
                {mode === 'signup' && (
                  <p className="text-xs text-muted-foreground">{t('auth.passwordHint')}</p>
                )}
              </div>
            )}

            {isResetting && resetRequested && (
              <>
                <p className="rounded-md border border-primary/30 bg-accent px-3 py-2 text-sm text-accent-foreground">
                  {t('auth.resetSent')}
                </p>

                <div className="grid gap-2">
                  <Label htmlFor="resetToken">{t('auth.resetToken')}</Label>
                  <Input
                    id="resetToken"
                    required
                    placeholder={t('auth.resetTokenPlaceholder')}
                    value={resetToken}
                    onChange={(event) => setResetToken(event.target.value)}
                  />
                </div>

                <div className="grid gap-2">
                  <Label htmlFor="newPassword">{t('auth.newPassword')}</Label>
                  <Input
                    id="newPassword"
                    type="password"
                    autoComplete="new-password"
                    required
                    minLength={6}
                    value={newPassword}
                    onChange={(event) => setNewPassword(event.target.value)}
                  />
                  <p className="text-xs text-muted-foreground">{t('auth.passwordHint')}</p>
                </div>
              </>
            )}

            {message !== null && (
              <p className="rounded-md border border-primary/30 bg-accent px-3 py-2 text-sm text-accent-foreground">
                {message}
              </p>
            )}

            {errors.length > 0 && (
              <ul
                role="alert"
                className="grid gap-1 rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive"
              >
                {errors.map((text) => (
                  <li key={text}>{text}</li>
                ))}
              </ul>
            )}

            <Button type="submit" disabled={isSubmitting} className="w-full">
              {isSubmitting && <Loader2 className="size-4 animate-spin" />}
              {submitLabel}
            </Button>

            {!isResetting && mode === 'signin' && (
              <Button
                type="button"
                variant="link"
                className="h-auto w-fit self-center p-0"
                onClick={() => {
                  setIsResetting(true)
                  setFailure(null)
                  setMessage(null)
                }}
              >
                {t('auth.forgotPassword')}
              </Button>
            )}
          </form>
        </CardContent>
      </Card>
    </div>
  )
}
