# PeakSet

> Training & Progress Tracker — tu diario de entrenamiento. Cada serie cuenta.

[![CI](https://github.com/Sergtran/PeakSet/actions/workflows/ci.yml/badge.svg)](https://github.com/Sergtran/PeakSet/actions/workflows/ci.yml)

![PeakSet](icon-512.png)

Aplicación full-stack de seguimiento de entrenamientos: rutinas, sesiones, historial, PRs y estadísticas en la nube.

## Características

- **Rutinas y ciclos** con seguimiento semanal y barra de progreso
- **Sesiones de entrenamiento**: series, repeticiones, peso y ejercicios unilaterales/bilaterales
- **Historial y calendario** para consultar y clonar sesiones pasadas
- **PRs y estadísticas** con gráficas de evolución
- **Interval timer** integrado
- **Sincronización en la nube** con cuenta de Google (PWA instalable)
- Tema claro/oscuro y soporte móvil

## Stack

- **Frontend:** PWA en HTML/CSS/JS (Firebase Auth + Firestore, Chart.js)
- **Backend:** C# / ASP.NET Core 8, Clean Architecture + DDD
- **Datos:** EF Core + PostgreSQL
- **Auth API:** Identity + JWT (API-first)
- **Infra:** Docker + GitHub Actions (CI/CD)
- **Tests:** 35+ tests con xUnit

## Estructura

```text
.
├── index.html / manifest.json     # PWA cliente (PeakSet)
├── Migration/PeakSet/                  # API .NET 8
└── docker-compose.yml             # PostgreSQL + API
```

## Cómo correrla

```bash
docker compose up -d
```
