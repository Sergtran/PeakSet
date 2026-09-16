import { ApiError, type ApiErrorItem } from '../api/client'
import type { Translate } from './context'
import type { TranslationKey } from './translations'

const codeTranslations: Record<string, TranslationKey> = {
  EmailRequired: 'errors.emailRequired',
  EmailInvalid: 'errors.emailInvalid',
  PasswordRequired: 'errors.passwordRequired',
  PasswordTooShort: 'errors.passwordLength',
  NameTooLong: 'errors.nameTooLong',
  RoutineNameRequired: 'errors.nameRequired',
  SessionNameRequired: 'errors.nameRequired',
  ExerciseNameRequired: 'errors.nameRequired',
  ExerciseTypeInvalid: 'errors.exerciseTypeInvalid',
  LateralityInvalid: 'errors.lateralityInvalid',
  ExerciseSetsRequired: 'errors.setsRequired',
  WorkoutExercisesRequired: 'errors.exercisesRequired',
  WorkoutDateRequired: 'errors.workoutDateRequired',
  SetRepsInvalid: 'errors.setRepsInvalid',
  SetWeightInvalid: 'errors.setWeightInvalid',
  InvalidCredentials: 'errors.invalidCredentials',
  NotFound: 'errors.notFound',
  DomainRule: 'errors.domainRule',
  Unexpected: 'errors.unexpected',
  ValidationFailed: 'errors.validation',
  UnsupportedBackupVersion: 'errors.backupInvalid',
  DuplicateRoutineIds: 'errors.backupInvalid',
  CalendarYearOutOfRange: 'errors.calendarRange',
  CalendarMonthOutOfRange: 'errors.calendarRange',
  ThemeInvalid: 'errors.settingsInvalid',
  SettingsPrepInvalid: 'errors.settingsInvalid',
  SettingsWorkInvalid: 'errors.settingsInvalid',
  SettingsRestInvalid: 'errors.settingsInvalid',
  SettingsSetsInvalid: 'errors.settingsInvalid',

  PasswordRequiresNonAlphanumeric: 'errors.passwordNonAlphanumeric',
  PasswordRequiresDigit: 'errors.passwordDigit',
  PasswordRequiresUpper: 'errors.passwordUppercase',
  PasswordRequiresLower: 'errors.passwordLowercase',
  PasswordRequiresUniqueChars: 'errors.passwordUniqueChars',
  DuplicateUserName: 'errors.emailTaken',
  DuplicateEmail: 'errors.emailTaken',
  InvalidToken: 'errors.invalidResetToken',
  ResetTokenRequired: 'errors.resetTokenRequired',
}

function translateItem(item: ApiErrorItem, t: Translate): string {
  const key = codeTranslations[item.code]
  return key ? t(key) : item.message
}

/** Distinct codes can describe the same problem, so the list is deduplicated. */
function dedupe(messages: string[]): string[] {
  return Array.from(new Set(messages))
}

export function describeApiError(error: unknown, t: Translate): string[] {
  if (!(error instanceof ApiError)) {
    return [t('errors.unexpected')]
  }
  if (error.items.length > 0) {
    return dedupe(error.items.map((item) => translateItem(item, t)))
  }
  if (error.status === 0) {
    return [t('errors.network')]
  }
  if (error.status === 401) {
    return [t('errors.sessionExpired')]
  }
  if (error.message.trim()) {
    return [error.message]
  }
  return [t('errors.httpStatus', { status: error.status })]
}
