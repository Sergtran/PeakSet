const API_BASE_URL: string = import.meta.env.VITE_API_BASE_URL ?? '/api'

export type ApiErrorItem = {
  code: string
  message: string
}

export class ApiError extends Error {
  readonly status: number
  readonly items: readonly ApiErrorItem[]

  constructor(status: number, message: string, items: readonly ApiErrorItem[] = []) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.items = items
  }
}

type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
  body?: unknown
  token?: string | null
}

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
    throw new ApiError(0, '')
  }

  const payload = await parseBody(response)

  if (!response.ok) {
    const problem = readProblem(payload)
    throw new ApiError(response.status, problem.message, problem.items)
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

type Problem = { message: string; items: ApiErrorItem[] }

function readProblem(payload: unknown): Problem {
  if (typeof payload === 'string' && payload.trim()) {
    return { message: payload.trim(), items: [] }
  }

  if (payload && typeof payload === 'object') {
    const problem = payload as { detail?: unknown; errors?: unknown; title?: unknown }
    const items = Array.isArray(problem.errors)
      ? problem.errors.map(readErrorItem).filter((item): item is ApiErrorItem => item !== null)
      : []

    if (items.length > 0) {
      return { message: items.map((item) => item.message).join(' '), items }
    }
    if (typeof problem.detail === 'string' && problem.detail.trim()) {
      return { message: problem.detail.trim(), items: [] }
    }
    if (typeof problem.title === 'string' && problem.title.trim()) {
      return { message: problem.title.trim(), items: [] }
    }
  }

  return { message: '', items: [] }
}

function readErrorItem(entry: unknown): ApiErrorItem | null {
  if (typeof entry === 'string') {
    return entry.trim() ? { code: '', message: entry.trim() } : null
  }
  if (entry && typeof entry === 'object') {
    const item = entry as { code?: unknown; message?: unknown }
    if (typeof item.message === 'string' && item.message.trim()) {
      return {
        code: typeof item.code === 'string' ? item.code : '',
        message: item.message.trim(),
      }
    }
  }
  return null
}
