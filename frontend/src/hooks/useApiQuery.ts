import { useCallback, useEffect, useRef, useState } from 'react'
import { ApiError } from '../api/client'
import { useAuth } from '../auth/context'

type QueryState<T> = {
  status: 'loading' | 'ready' | 'error'
  data: T | null
  error: unknown
}

export type ApiQueryResult<T> = {
  data: T | null
  error: unknown
  isLoading: boolean
  reload: () => void
}

/**
 * `key` identifies the request. When it changes the query runs again, so callers
 * pass something like `routine:${routineId}` instead of a dependency array.
 */
export function useApiQuery<T>(
  key: string,
  load: (token: string) => Promise<T>,
): ApiQueryResult<T> {
  const { token, signOut } = useAuth()
  const [state, setState] = useState<QueryState<T>>({
    status: 'loading',
    data: null,
    error: null,
  })
  const [attempt, setAttempt] = useState(0)
  const loadRef = useRef(load)

  useEffect(() => {
    loadRef.current = load
  }, [load])

  useEffect(() => {
    let isActive = true

    loadRef
      .current(token)
      .then((data) => {
        if (isActive) {
          setState({ status: 'ready', data, error: null })
        }
      })
      .catch((error: unknown) => {
        if (!isActive) return
        if (error instanceof ApiError && error.status === 401) {
          signOut()
          return
        }
        setState({ status: 'error', data: null, error })
      })

    return () => {
      isActive = false
    }
  }, [token, key, attempt, signOut])

  const reload = useCallback(() => {
    setState({ status: 'loading', data: null, error: null })
    setAttempt((current) => current + 1)
  }, [])

  return {
    data: state.data,
    error: state.error,
    isLoading: state.status === 'loading',
    reload,
  }
}
