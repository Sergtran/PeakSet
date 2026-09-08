# GymTracker

[![CI](https://github.com/Sergtran/GymTracker/actions/workflows/ci.yml/badge.svg)](https://github.com/Sergtran/GymTracker/actions/workflows/ci.yml)

Aplicación de seguimiento de entrenamientos: rutinas, sesiones, historial, PRs y estadísticas.

- **Backend:** C# / ASP.NET Core 8, Clean Architecture + DDD
- **Datos:** EF Core + PostgreSQL
- **Auth:** Identity + JWT (API-first)
- **Infra:** Docker + GitHub Actions (CI/CD)
- **Tests:** 35+ tests xUnit

## Cómo correrla

```bash
docker compose up -d