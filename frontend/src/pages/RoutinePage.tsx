import { useState } from 'react'
import {
  createSession,
  createSessionExercise,
  deleteRoutine,
  deleteSession,
  deleteSessionExercise,
  fetchRoutine,
  renameRoutine,
  renameSession,
  type Session,
} from '../api/routines'
import { exerciseTypes, lateralities, type ExerciseType, type Laterality } from '../api/types'
import { useAuth } from '../auth/context'
import { ErrorList, LoadingState } from '../components/Feedback'
import { InlineNameForm } from '../components/InlineNameForm'
import { ScreenHeader } from '../components/ScreenHeader'
import { useApiQuery } from '../hooks/useApiQuery'
import { useConfirm } from '../hooks/useConfirm'
import { useI18n } from '../i18n/context'
import { useNavigation } from '../navigation/context'

export function RoutinePage({ routineId }: { routineId: string }) {
  const { t } = useI18n()
  const { token } = useAuth()
  const { navigate, back } = useNavigation()
  const detail = useApiQuery(`routine:${routineId}`, (token) => fetchRoutine(token, routineId))
  const { confirm, dialog } = useConfirm()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<unknown>(null)
  const [isRenaming, setIsRenaming] = useState(false)

  const routine = detail.data

  async function run(action: () => Promise<unknown>) {
    setBusy(true)
    setError(null)
    try {
      await action()
      detail.reload()
    } catch (cause) {
      setError(cause)
    } finally {
      setBusy(false)
    }
  }

  async function handleDeleteRoutine(name: string) {
    const accepted = await confirm({
      title: t('routine.deleteRoutine'),
      body: t('routine.confirmDeleteRoutine', { name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) {
      await deleteRoutine(token, routineId)
      back()
    }
  }

  async function handleDeleteSession(sessionId: string, name: string) {
    const accepted = await confirm({
      title: t('common.delete'),
      body: t('routine.confirmDeleteSession', { name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) {
      await run(() => deleteSession(token, routineId, sessionId))
    }
  }

  async function handleDeleteExercise(sessionId: string, exerciseId: string, name: string) {
    const accepted = await confirm({
      title: t('common.delete'),
      body: t('routine.confirmDeleteExercise', { name }),
      confirmLabel: t('common.delete'),
      danger: true,
    })
    if (accepted) {
      await run(() => deleteSessionExercise(token, routineId, sessionId, exerciseId))
    }
  }

  if (detail.error !== null) {
    return (
      <>
        <ScreenHeader title={t('menu.routines')} />
        <ErrorList error={detail.error} />
      </>
    )
  }

  if (!routine) {
    return (
      <>
        <ScreenHeader title={t('menu.routines')} />
        <LoadingState />
      </>
    )
  }

  return (
    <>
      <ScreenHeader title={routine.name} subtitle={t('routine.sessions')} />

      {error !== null && <ErrorList error={error} />}

      <div className="button-row">
        <button
          className="btn btn-ghost"
          type="button"
          onClick={() => setIsRenaming((value) => !value)}
        >
          {t('common.edit')}
        </button>
        <button
          className="btn btn-danger"
          type="button"
          disabled={busy}
          onClick={() => void handleDeleteRoutine(routine.name)}
        >
          {t('routine.deleteRoutine')}
        </button>
      </div>

      {isRenaming && (
        <InlineNameForm
          initialValue={routine.name}
          placeholder={t('routines.namePlaceholder')}
          submitLabel={t('common.save')}
          autoFocus
          disabled={busy}
          onCancel={() => setIsRenaming(false)}
          onSubmit={(name) => {
            setIsRenaming(false)
            void run(() => renameRoutine(token, routine.id, name))
          }}
        />
      )}

      <InlineNameForm
        placeholder={t('routine.sessionNamePlaceholder')}
        submitLabel={t('routine.addSession')}
        disabled={busy}
        onSubmit={(name) => void run(() => createSession(token, routine.id, name))}
      />

      <div className="card-list">
        {routine.sessions.map((session) => (
          <SessionCard
            key={session.id}
            session={session}
            onTrain={() =>
              navigate({
                name: 'session',
                routineId: routine.id,
                sessionId: session.id,
                sessionName: session.name,
              })
            }
            onRename={(name) => void run(() => renameSession(token, routine.id, session.id, name))}
            onDelete={() => void handleDeleteSession(session.id, session.name)}
            onAddExercise={(request) =>
              run(() => createSessionExercise(token, routine.id, session.id, request))
            }
            onDeleteExercise={(exerciseId, name) =>
              void handleDeleteExercise(session.id, exerciseId, name)
            }
          />
        ))}
      </div>

      {dialog}
    </>
  )
}

type SessionCardProps = {
  session: Session
  onTrain: () => void
  onRename: (name: string) => void
  onDelete: () => void
  onAddExercise: (request: {
    name: string
    exerciseType: ExerciseType
    laterality: Laterality
  }) => Promise<unknown>
  onDeleteExercise: (exerciseId: string, name: string) => void
}

function SessionCard({
  session,
  onTrain,
  onRename,
  onDelete,
  onAddExercise,
  onDeleteExercise,
}: SessionCardProps) {
  const { t } = useI18n()
  const [isRenaming, setIsRenaming] = useState(false)
  const [isAdding, setIsAdding] = useState(false)
  const [name, setName] = useState('')
  const [exerciseType, setExerciseType] = useState<ExerciseType>('Standard')
  const [laterality, setLaterality] = useState<Laterality>('Bilateral')

  function submitExercise() {
    const trimmed = name.trim()
    if (!trimmed) return
    void onAddExercise({ name: trimmed, exerciseType, laterality })
    setName('')
    setIsAdding(false)
  }

  return (
    <article className="card session-card">
      <div className="list-row">
        <div className="list-row-main">
          <h2 className="list-row-title">{session.name}</h2>
          <p className="list-row-meta">{t('routine.exercises')}</p>
        </div>
        {!isRenaming && (
          <div className="button-row">
            <button className="btn btn-ghost" type="button" onClick={() => setIsRenaming(true)}>
              {t('common.edit')}
            </button>
            <button className="btn btn-primary" type="button" onClick={onTrain}>
              {t('routine.startSession')}
            </button>
            <button className="btn btn-danger" type="button" onClick={onDelete}>
              {t('common.delete')}
            </button>
          </div>
        )}
      </div>

      {isRenaming && (
        <InlineNameForm
          initialValue={session.name}
          placeholder={t('routine.sessionNamePlaceholder')}
          submitLabel={t('common.save')}
          autoFocus
          onCancel={() => setIsRenaming(false)}
          onSubmit={(value) => {
            setIsRenaming(false)
            onRename(value)
          }}
        />
      )}

      {session.exercises.length === 0 && <p className="list-row-meta">{t('routine.noExercises')}</p>}

      <ul className="exercise-list">
        {session.exercises.map((exercise) => (
          <li key={exercise.id} className="exercise-row">
            <span className="exercise-name">{exercise.name}</span>
            <span className="exercise-meta">
              {t(`exerciseType.${exercise.exerciseType}`)} · {t(`laterality.${exercise.laterality}`)}
            </span>
            <button
              className="btn btn-ghost"
              type="button"
              onClick={() => onDeleteExercise(exercise.id, exercise.name)}
            >
              ✕
            </button>
          </li>
        ))}
      </ul>

      {isAdding ? (
        <div className="exercise-form">
          <input
            className="field-input"
            placeholder={t('routine.exerciseNamePlaceholder')}
            value={name}
            autoFocus
            maxLength={150}
            onChange={(event) => setName(event.target.value)}
          />
          <select
            className="field-input"
            value={exerciseType}
            onChange={(event) => setExerciseType(event.target.value as ExerciseType)}
          >
            {exerciseTypes.map((type) => (
              <option key={type} value={type}>
                {t(`exerciseType.${type}`)}
              </option>
            ))}
          </select>
          <select
            className="field-input"
            value={laterality}
            onChange={(event) => setLaterality(event.target.value as Laterality)}
          >
            {lateralities.map((value) => (
              <option key={value} value={value}>
                {t(`laterality.${value}`)}
              </option>
            ))}
          </select>
          <div className="button-row">
            <button
              className="btn btn-primary"
              type="button"
              disabled={!name.trim()}
              onClick={submitExercise}
            >
              {t('common.add')}
            </button>
            <button className="btn btn-ghost" type="button" onClick={() => setIsAdding(false)}>
              {t('common.cancel')}
            </button>
          </div>
        </div>
      ) : (
        <button className="btn btn-ghost" type="button" onClick={() => setIsAdding(true)}>
          + {t('routine.addExercise')}
        </button>
      )}
    </article>
  )
}
