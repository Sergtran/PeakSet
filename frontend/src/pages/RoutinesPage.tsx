import { useId, useState } from 'react'
import {
  ChevronDown,
  Dumbbell,
  Pencil,
  Plus,
  Star,
  Trash2,
} from 'lucide-react'
import { fetchHome } from '@/api/home'
import { fetchExercises } from '@/api/exercises'
import {
  createSession,
  createSessionExercise,
  createRoutine,
  deleteRoutine,
  deleteSession,
  deleteSessionExercise,
  fetchRoutine,
  fetchRoutines,
  renameRoutine,
  type Routine,
  type Session,
} from '@/api/routines'
import { exerciseTypes, lateralities, type ExerciseType, type Laterality } from '@/api/types'
import { setCurrentRoutine } from '@/api/users'
import { useAuth } from '@/auth/context'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { Skeleton } from '@/components/ui/skeleton'
import { useApiQuery } from '@/hooks/useApiQuery'
import { useConfirm } from '@/hooks/useConfirm'
import { useI18n } from '@/i18n/context'
import { useNavigation } from '@/navigation/context'

export function RoutinesPage({ expand }: { expand?: string }) {
  const { t } = useI18n()
  const { token } = useAuth()
  const { confirm, dialog } = useConfirm()
  const routines = useApiQuery('routines', (token) => fetchRoutines(token))
  const home = useApiQuery('home', (token) => fetchHome(token))
  const [openId, setOpenId] = useState<string | null>(expand ?? null)
  const [name, setName] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const currentId = home.data?.currentRoutine?.routineId ?? null

  async function run(action: () => Promise<unknown>) {
    setBusy(true)
    setError(null)
    try {
      await action()
      routines.reload()
      home.reload()
    } catch {
      setError(t('settings.saveFailed'))
    } finally {
      setBusy(false)
    }
  }

  async function handleCreate() {
    const trimmed = name.trim()
    if (!trimmed) return
    setName('')
    await run(async () => {
      const created = await createRoutine(token, trimmed)
      setOpenId(created.id)
    })
  }

  async function handleDelete(routine: Routine) {
    const accepted = await confirm({
      title: t('routine.deleteRoutine'),
      body: t('routine.confirmDeleteRoutine', { name: routine.name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) await run(() => deleteRoutine(token, routine.id))
  }

  return (
    <div className="grid gap-5">
      <header className="grid gap-1">
        <h1 className="text-2xl font-semibold tracking-tight">{t('menu.routines')}</h1>
        <p className="text-sm text-muted-foreground">{t('routines.emptyBody')}</p>
      </header>

      <Card>
        <CardContent className="flex gap-2 pt-6">
          <Input
            value={name}
            maxLength={100}
            placeholder={t('routines.namePlaceholder')}
            onChange={(event) => setName(event.target.value)}
            onKeyDown={(event) => {
              if (event.key === 'Enter') void handleCreate()
            }}
          />
          <Button disabled={busy || !name.trim()} onClick={() => void handleCreate()}>
            <Plus className="size-4" />
            <span className="hidden sm:inline">{t('routines.new')}</span>
          </Button>
        </CardContent>
      </Card>

      {error !== null && (
        <p className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive">
          {error}
        </p>
      )}

      {routines.isLoading && <Skeleton className="h-20 w-full rounded-xl" />}

      <div className="grid gap-3">
        {routines.data?.map((routine) => {
          const open = openId === routine.id
          return (
            <Card key={routine.id}>
              <button
                type="button"
                className="flex w-full items-center gap-3 px-4 py-4 text-left"
                onClick={() => setOpenId(open ? null : routine.id)}
              >
                <ChevronDown
                  className={
                    open
                      ? 'size-4 shrink-0 text-muted-foreground transition-transform rotate-180'
                      : 'size-4 shrink-0 text-muted-foreground transition-transform'
                  }
                />
                <span className="flex-1 truncate font-medium">{routine.name}</span>
                {routine.id === currentId && <Badge variant="secondary">{t('routines.current')}</Badge>}
              </button>

              {open && (
                <CardContent className="grid gap-4 pb-5">
                  <div className="flex flex-wrap gap-2">
                    {routine.id !== currentId && (
                      <Button
                        variant="outline"
                        size="sm"
                        disabled={busy}
                        onClick={() => void run(() => setCurrentRoutine(token, routine.id))}
                      >
                        <Star className="size-4" />
                        {t('routines.makeCurrent')}
                      </Button>
                    )}
                    <RenameButton
                      initial={routine.name}
                      busy={busy}
                      onRename={(value) => run(() => renameRoutine(token, routine.id, value))}
                    />
                    <Button
                      variant="outline"
                      size="sm"
                      className="text-destructive"
                      disabled={busy}
                      onClick={() => void handleDelete(routine)}
                    >
                      <Trash2 className="size-4" />
                      {t('common.delete')}
                    </Button>
                  </div>

                  <Separator />

                  <SessionsEditor routineId={routine.id} />
                </CardContent>
              )}
            </Card>
          )
        })}
      </div>

      {dialog}
    </div>
  )
}

function RenameButton({
  initial,
  busy,
  onRename,
}: {
  initial: string
  busy: boolean
  onRename: (value: string) => Promise<unknown>
}) {
  const { t } = useI18n()
  const [editing, setEditing] = useState(false)
  const [value, setValue] = useState(initial)

  if (!editing) {
    return (
      <Button variant="outline" size="sm" onClick={() => setEditing(true)}>
        <Pencil className="size-4" />
        {t('common.rename')}
      </Button>
    )
  }

  async function save() {
    const trimmed = value.trim()
    if (!trimmed) return
    setEditing(false)
    await onRename(trimmed)
  }

  return (
    <div className="flex w-full gap-2">
      <Input value={value} maxLength={100} onChange={(event) => setValue(event.target.value)} />
      <Button size="sm" disabled={busy || !value.trim()} onClick={() => void save()}>
        {t('common.save')}
      </Button>
      <Button size="sm" variant="ghost" onClick={() => setEditing(false)}>
        {t('common.cancel')}
      </Button>
    </div>
  )
}

function SessionsEditor({ routineId }: { routineId: string }) {
  const { t } = useI18n()
  const { token } = useAuth()
  const { navigate } = useNavigation()
  const { confirm, dialog } = useConfirm()
  const detail = useApiQuery(`routine:${routineId}`, (token) => fetchRoutine(token, routineId))
  const [sessionName, setSessionName] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function run(action: () => Promise<unknown>) {
    setBusy(true)
    setError(null)
    try {
      await action()
      detail.reload()
    } catch {
      setError(t('settings.saveFailed'))
    } finally {
      setBusy(false)
    }
  }

  async function handleDeleteSession(session: Session) {
    const accepted = await confirm({
      title: t('common.delete'),
      body: t('routine.confirmDeleteSession', { name: session.name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) await run(() => deleteSession(token, routineId, session.id))
  }

  if (detail.isLoading) return <Skeleton className="h-16 w-full rounded-lg" />

  return (
    <div className="grid gap-4">
      {error !== null && (
        <p className="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive">
          {error}
        </p>
      )}

      {detail.data?.sessions.length === 0 && (
        <p className="text-sm text-muted-foreground">{t('routine.noSessionsBody')}</p>
      )}

      {detail.data?.sessions.map((session) => (
        <div key={session.id} className="grid gap-3 rounded-lg border p-3">
          <div className="flex items-center gap-2">
            <span className="flex-1 truncate text-sm font-medium">{session.name}</span>
            <Button
              size="sm"
              disabled={busy}
              onClick={() =>
                navigate({
                  name: 'session',
                  routineId,
                  sessionId: session.id,
                  sessionName: session.name,
                })
              }
            >
              <Dumbbell className="size-4" />
              {t('train.start')}
            </Button>
            <Button
              size="icon"
              variant="ghost"
              className="text-destructive"
              disabled={busy}
              aria-label={t('common.delete')}
              onClick={() => void handleDeleteSession(session)}
            >
              <Trash2 className="size-4" />
            </Button>
          </div>

          {session.exercises.map((exercise) => (
            <div
              key={exercise.id}
              className="flex items-center gap-2 rounded-md bg-secondary/60 px-3 py-2 text-sm"
            >
              <span className="flex-1 truncate">{exercise.name}</span>
              <span className="hidden text-xs text-muted-foreground sm:inline">
                {t(`exerciseType.${exercise.exerciseType}`)}
              </span>
              <Button
                size="icon"
                variant="ghost"
                className="size-7 text-destructive"
                aria-label={t('common.delete')}
                disabled={busy}
                onClick={() =>
                  void run(() =>
                    deleteSessionExercise(token, routineId, session.id, exercise.id),
                  )
                }
              >
                <Trash2 className="size-3.5" />
              </Button>
            </div>
          ))}

          <ExerciseForm
            busy={busy}
            onSubmit={(request) =>
              run(() => createSessionExercise(token, routineId, session.id, request))
            }
          />
        </div>
      ))}

      <div className="flex gap-2">
        <Input
          value={sessionName}
          maxLength={100}
          placeholder={t('routine.sessionNamePlaceholder')}
          onChange={(event) => setSessionName(event.target.value)}
        />
        <Button
          variant="outline"
          disabled={busy || !sessionName.trim()}
          onClick={() => {
            const trimmed = sessionName.trim()
            setSessionName('')
            void run(() => createSession(token, routineId, trimmed))
          }}
        >
          <Plus className="size-4" />
          {t('routine.addSession')}
        </Button>
      </div>

      {dialog}
    </div>
  )
}

type ExerciseRequest = {
  name: string
  exerciseType: ExerciseType
  laterality: Laterality
}

function ExerciseForm({
  busy,
  onSubmit,
}: {
  busy: boolean
  onSubmit: (request: ExerciseRequest) => Promise<unknown>
}) {
  const { t } = useI18n()
  const nameId = useId()
  const suggestions = useApiQuery('exercises', (token) => fetchExercises(token))
  const [open, setOpen] = useState(false)
  const [name, setName] = useState('')
  const [exerciseType, setExerciseType] = useState<ExerciseType>('Standard')
  const [laterality, setLaterality] = useState<Laterality>('Bilateral')

  if (!open) {
    return (
      <Button variant="ghost" size="sm" className="w-fit" onClick={() => setOpen(true)}>
        <Plus className="size-4" />
        {t('routine.addExercise')}
      </Button>
    )
  }

  async function submit() {
    const trimmed = name.trim()
    if (!trimmed) return
    setName('')
    setOpen(false)
    await onSubmit({ name: trimmed, exerciseType, laterality })
  }

  return (
    <div className="grid gap-3 rounded-md border border-dashed p-3">
      <div className="grid gap-2">
        <Label htmlFor={nameId}>{t('routine.exerciseNamePlaceholder')}</Label>
        <Input
          id={nameId}
          list={`${nameId}-list`}
          value={name}
          maxLength={150}
          autoFocus
          onChange={(event) => setName(event.target.value)}
        />
        <datalist id={`${nameId}-list`}>
          {suggestions.data?.map((item) => (
            <option key={item.name} value={item.name} />
          ))}
        </datalist>
      </div>

      <div className="grid grid-cols-2 gap-3">
        <div className="grid gap-2">
          <Label>{t('routine.exerciseType')}</Label>
          <Select
            value={exerciseType}
            onValueChange={(value) => setExerciseType(value as ExerciseType)}
          >
            <SelectTrigger className="w-full">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              {exerciseTypes.map((type) => (
                <SelectItem key={type} value={type}>
                  {t(`exerciseType.${type}`)}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="grid gap-2">
          <Label>{t('routine.laterality')}</Label>
          <Select
            value={laterality}
            onValueChange={(value) => setLaterality(value as Laterality)}
          >
            <SelectTrigger className="w-full">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              {lateralities.map((value) => (
                <SelectItem key={value} value={value}>
                  {t(`laterality.${value}`)}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="flex gap-2">
        <Button size="sm" disabled={busy || !name.trim()} onClick={() => void submit()}>
          {t('common.add')}
        </Button>
        <Button size="sm" variant="ghost" onClick={() => setOpen(false)}>
          {t('common.cancel')}
        </Button>
      </div>
    </div>
  )
}
