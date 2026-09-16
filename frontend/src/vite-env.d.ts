/// <reference types="vite/client" />

/**
 * Environment variables that Vite exposes to the browser.
 * They are declared explicitly so TypeScript knows their names and types.
 */
interface ImportMetaEnv {
  /** Base URL of the PeakSet API. Defaults to "/api" (see vite.config.ts). */
  readonly VITE_API_BASE_URL?: string
}
