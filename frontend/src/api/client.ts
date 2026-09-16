/**
 * The only place in the app that knows how to talk to the PeakSet API.
 *
 * Every screen goes through `apiRequest`, so the base URL, the
 * `Authorization: Bearer` header and all error handling live here exactly once.
 */

/**
 * During development this stays as "/api" and `vite.config.ts` proxies the
 * request to the deployed API. In production we set VITE_API_BASE_URL to the
 * absolute URL of the API (see .env.example).
 */
const API_BASE_URL: string = import.meta.env.VITE_API_BASE_URL ?? '/api'

/** An error coming from the API (or from the network itself). */
export class ApiError extends Error {
  /** HTTP status code. `0` means we never got a response (offline, DNS, ...). */
  readonly status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
  /** Plain object: it is serialized to JSON for you. */
  body?: unknown
  /** JWT of the signed in user. Omitted for public endpoints such as login. */
  token?: string | null
}

/**
 * Calls the API and returns the parsed JSON body.
 * Throws an `ApiError` when the response is not 2xx.
 */
export async function apiRequest<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { method = 'GET', body, token } = options

  const headers: Record<string, string> = { Accept: 'application/json' }
  if (body !== undefined) {
    headers['Content-Type'] = 'application/json'
  }
  if (token) {
    headers.Authorization = `Bearer ${token}`
  }

  let response: Response
  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      method,
      headers,
      body: body === undefined ? undefined : JSON.stringify(body),
    })
  } catch {
    throw new ApiError(0, 'Could not reach the PeakSet API. Check your connection and try again.')
  }

  const payload = await parseBody(response)

  if (!response.ok) {
    throw new ApiError(response.status, describeError(payload, response.status))
  }

  return payload as T
}

async function parseBody(response: Response): Promise<unknown> {
  const text = await response.text()
  if (!text) {
    return null
  }
  try {
    return JSON.parse(text)
  } catch {
    return text
  }
}

/**
 * The backend returns RFC 7807 problem details, for example:
 * `{ "status": 400, "title": "Validation failed", "detail": "Password must be at least 6 characters long." }`
 */
function describeError(payload: unknown, status: number): string {
  if (typeof payload === 'string' && payload.trim()) {
    return payload.trim()
  }

  if (payload && typeof payload === 'object') {
    const problem = payload as { detail?: unknown; errors?: unknown; title?: unknown }

    if (typeof problem.detail === 'string' && problem.detail.trim()) {
      return problem.detail.trim()
    }
    if (Array.isArray(problem.errors) && problem.errors.length > 0) {
      return problem.errors.join(' ')
    }
    if (typeof problem.title === 'string' && problem.title.trim()) {
      return problem.title.trim()
    }
  }

  if (status === 401) {
    return 'Your session has expired. Please sign in again.'
  }
  return `The request failed (HTTP ${status}).`
}
