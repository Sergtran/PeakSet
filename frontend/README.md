# PeakSet Web

React + TypeScript single page application for PeakSet.

## Requirements

- Node.js 20 or newer
- The PeakSet API reachable at `https://peakset-api.azurewebsites.net`

## Getting started

```bash
npm install
npm run dev
```

Then open the URL that Vite prints, by default <http://localhost:5173>.

## Scripts

| Command | Description |
| --- | --- |
| `npm run dev` | Development server with hot reload. |
| `npm run build` | Type-checks the project and builds it into `dist/`. |
| `npm run preview` | Serves the production build locally. |
| `npm run lint` | Runs oxlint. |

## How it talks to the API

`src/api/client.ts` is the only module that performs HTTP calls. It owns the
base URL, the `Authorization: Bearer <token>` header and error parsing, so
screens never build requests by hand.

The base URL comes from `VITE_API_BASE_URL`. In development it defaults to
`/api`, and `vite.config.ts` proxies those requests to the deployed API. The
browser therefore only ever sees `localhost`, which avoids CORS entirely.

Copy `.env.example` to `.env` to override the default.

## Project structure

```
src/
  api/          HTTP layer: client, auth endpoints, home endpoint
  auth/         Session storage (JWT + profile in localStorage)
  pages/        One file per screen
  App.tsx       Picks the screen to render based on the session
  main.tsx      Entry point: mounts App into index.html
```

## Screens

1. **Login** — sign in and sign up against `POST /api/auth/login` and
   `POST /api/auth/register`. The returned JWT is stored so the session
   survives a page refresh.
2. **Dashboard** — calls `GET /api/home` with the JWT and shows the summary of
   the current routine. Expired tokens send the user back to the login screen.

## Deployment

The build output in `dist/` is a static folder and can be hosted on Firebase
Hosting, Azure Static Web Apps or any static host. Remember to set
`VITE_API_BASE_URL` at build time, and to allow the hosting origin in the API's
CORS configuration.
