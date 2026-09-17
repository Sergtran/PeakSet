import path from 'node:path'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(import.meta.dirname, './src'),
    },
  },
  server: {
    port: 5173,
    // The browser only ever talks to Vite. Vite forwards every /api/* call to
    // the real backend, so we never have to fight CORS while developing.
    proxy: {
      '/api': {
        target: 'https://peakset-api.azurewebsites.net',
        changeOrigin: true,
      },
    },
  },
})
