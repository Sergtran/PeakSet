import { useEffect, useState } from 'react'
import { ApiError } from '../api/client'
import { fetchHome, type Home } from '../api/home'
import type { Profile } from '../auth/tokenStorage'

type DashboardPageProps = {
  token: string
  profile: Profile
  onSignOut: () => void
}

/**
 * Screen 2: calls GET /api/home with the JWT in the
 * `Authorization: Bearer <token>` header.
 */
export function DashboardPage({ token, profile, onSignOut }: DashboardPageProps) {
  const [home, setHome] = useState<Home | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    // React runs effects twice in development. `isActive` makes sure we only
    // apply the result of the request that is still relevant.
    let isActive = true

    fetchHome(token)
      .then((data) => {
        if (!isActive) return
        setHome(data)
        setError(null)
      })
      .catch((cause: unknown) => {
        if (!isActive) return

        // A 401 means the stored token is no longer valid, so we sign the user out.
        if (cause instanceof ApiError && cause.status === 401) {
          onSignOut()
          return
        }

        setError(
          cause instanceof ApiError ? cause.message : 'Could not load your dashboard.',
        )
      })

    return () => {
      isActive = false
    }
  }, [token, onSignOut])

  // While the request is in flight we have neither data nor an error yet.
  const isLoading = home === null && error === null
  const routine = home?.currentRoutine ?? null

  return (
    <div className="app-shell">
      <header className="app-header">
        <div className="app-header-brand">
          <img
            className="brand-mark brand-mark-sm"
            src="/logo-192.png"
            alt=""
            width={32}
            height={32}
          />
          <span className="brand-name brand-name-sm">PeakSet</span>
        </div>

        <div className="app-header-user">
          <span className="user-name">{profile.displayName || profile.email}</span>
          <button className="btn btn-ghost" type="button" onClick={onSignOut}>
            Sign out
          </button>
        </div>
      </header>

      <main className="app-main">
        <h1 className="page-title">Today</h1>
        <p className="page-subtitle">
          {routine ? `Current routine: ${routine.name}` : 'Your training at a glance.'}
        </p>

        {isLoading && <p className="state-message">Loading your dashboard…</p>}

        {!isLoading && error && (
          <p className="alert alert-error" role="alert">
            {error}
          </p>
        )}

        {!isLoading && !error && !routine && (
          <section className="card empty-state">
            <h2 className="empty-title">No current routine yet</h2>
            <p className="empty-body">
              Once you create a routine and mark it as current, your workouts,
              records and weekly activity will show up here.
            </p>
          </section>
        )}

        {!isLoading && !error && routine && (
          <section className="stats-grid">
            <StatCard label="Workouts logged" value={routine.workoutCount} />
            <StatCard label="Personal records" value={routine.prCount} />
            <StatCard label="Weeks in use" value={routine.weeksInUse} />
            <StatCard
              label="Days since last workout"
              value={routine.daysSinceLastWorkout}
              highlight={routine.daysSinceLastWorkout >= 7}
            />
          </section>
        )}
      </main>
    </div>
  )
}

type StatCardProps = {
  label: string
  value: number
  highlight?: boolean
}

function StatCard({ label, value, highlight = false }: StatCardProps) {
  return (
    <article className={highlight ? 'card stat-card stat-card-warn' : 'card stat-card'}>
      <span className="stat-value">{value}</span>
      <span className="stat-label">{label}</span>
    </article>
  )
}
