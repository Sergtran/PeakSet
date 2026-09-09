# PeakSet

> Training & Progress Tracker — tu diario de entrenamiento. Cada serie cuenta.

[![CI/CD](https://github.com/Sergtran/PeakSet/actions/workflows/ci.yml/badge.svg)](https://github.com/Sergtran/PeakSet/actions/workflows/ci.yml)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![Azure](https://img.shields.io/badge/Azure-App%20Service-0078D4?logo=microsoftazure)](https://azure.microsoft.com/)

![PeakSet](icon-512.png)

Aplicación full-stack de seguimiento de entrenamientos: rutinas, sesiones, historial, PRs y estadísticas en la nube. Cuenta con una **API ASP.NET Core 8** desplegada en Azure (Clean Architecture + DDD, EF Core + PostgreSQL, Identity + JWT) y una **PWA cliente** instalable.

---

## Qué es PeakSet

PeakSet nació como una app personal para dejar de perder el registro de los entrenamientos. Registra tu **rutina actual**, completa **sesiones** con series, repeticiones y peso (ejercicios unilaterales y bilaterales), y consulta **historial, calendario, PRs y estadísticas** con gráficas de evolución.

La versión 2 (rama actual) migra la lógica a una **API REST en .NET** con arquitectura limpia, autenticación JWT y PostgreSQL, lista para consumir desde cualquier cliente.

## Features

- **Rutinas y ciclos**: rutina actual con seguimiento semanal y barra de progreso
- **Sesiones de entrenamiento**: series, repeticiones, peso y ejercicios unilaterales/bilaterales
- **Historial y calendario**: consulta y clona sesiones pasadas
- **PRs y estadísticas**: gráficas de evolución por ejercicio
- **Interval timer** integrado
- **Sincronización en la nube** con cuenta (PWA instalable, tema claro/oscuro, soporte móvil)
- **API-first**: catálogo de ejercicios, rutinas, sesiones, workouts y exportación/importación de datos

## Stack

| Capa | Tecnología |
|---|---|
| Frontend | PWA en HTML/CSS/JS (Firebase Auth + Firestore, Chart.js) |
| Backend | C# / ASP.NET Core 8 — Clean Architecture + DDD |
| Datos | EF Core + PostgreSQL 16 |
| Auth | ASP.NET Identity + JWT (API-first) |
| Infra | Docker, GitHub Actions (CI/CD), Azure App Service + Azure Database for PostgreSQL |
| Tests | 35+ tests con xUnit |

## Arquitectura

La solución se divide en capas con dependencias dirigidas hacia el dominio:

```text
PeakSet.Domain         → Entidades y reglas de negocio (sin dependencias externas)
PeakSet.Application    → Casos de uso, DTOs, validadores (FluentValidation)
PeakSet.Infrastructure → EF Core, repositorios, Identity, JWT, migraciones
PeakSet.Api            → Controllers REST, middleware de errores, Swagger/OpenAPI
PeakSet.Tests          → Tests de dominio y aplicación (xUnit)
```

![Arquitectura general](Migration/PeakSet/docs/Arquitectura%20general.png)

![Arquitectura de la solución](Migration/PeakSet/docs/Arquitectura%20de%20la%20soluci%C3%B3n.png)

### Estructura del repositorio

```text
.
├── index.html / manifest.json     # PWA cliente (PeakSet)
├── Migration/PeakSet/             # API .NET 8 (Clean Architecture)
│   ├── src/                       # Domain, Application, Infrastructure, Api
│   ├── test/                      # PeakSet.Tests (xUnit)
│   └── Dockerfile
└── docker-compose.yml             # PostgreSQL 16 + API para desarrollo local
```

## Cómo correrla localmente

Requisitos: Docker Desktop y una copia de `.env` con las variables del proyecto.

```bash
docker compose up -d
```

La API queda en `http://localhost:8080` con Swagger en `/swagger`, y PostgreSQL en el puerto configurado.

Para desarrollo con hot reload:

```bash
cd Migration/PeakSet
dotnet run --project src/PeakSet.Api
```

## API en vivo

La API está desplegada en Azure App Service (Linux):

- **Base URL:** https://peakset-api.azurewebsites.net
- **Swagger / OpenAPI:** https://peakset-api.azurewebsites.net/swagger

Ejemplo de registro:

```bash
curl -X POST https://peakset-api.azurewebsites.net/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"tu@email.com","password":"TuClave!123","displayName":"Tu Nombre"}'
```

Ejemplo de login (responde con un token JWT):

```bash
curl -X POST https://peakset-api.azurewebsites.net/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"tu@email.com","password":"TuClave!123"}'
```

Usa el token en el header `Authorization: Bearer <token>` para los endpoints protegidos (`/api/routines`, `/api/workouts`, `/api/users`, `/api/exercises`, `/api/home`, `/api/data`, `/api/calendar`).

## Testing

```bash
cd Migration/PeakSet
dotnet test PeakSet.sln --configuration Release
```

**35/35 tests** con xUnit sobre el dominio y la aplicación (validadores, PRs, sesiones, rutinas).

## Azure

- **App Service (Linux, Free F1):** `peakset-api` — Canada Central
  - Runtime .NET 8, HTTPS disponible en `https://peakset-api.azurewebsites.net`
- **Azure Database for PostgreSQL 16 (Flexible Server):** `peakset-db` — Canada Central
  - SKU B1ms burstable, 32 GiB, conexión con SSL obligatorio
- Configuración por **variables de entorno** del App Service (connection string, JWT, flags de Swagger) — sin secretos en el repositorio
- Migraciones EF Core aplicadas en el pipeline de despliegue

## CI/CD

Pipeline único en [`.github/workflows/ci.yml`](.github/workflows/ci.yml):

```text
Push a main ──► build-test ──► deploy (OIDC) ──► Azure App Service
PR a main   ──► build-test
```

- **CI** (`build-test`): restaura, compila y corre los 35 tests en cada push y PR a `main`
- **CD** (`deploy`): publica la API y despliega a Azure App Service cuando el push a `main` pasa los tests
- **Seguridad**: autenticación con **GitHub Actions OIDC + federated credential** de Azure — no se almacenan credenciales de Azure de larga duración en GitHub

![Pipeline CI/CD en verde](Migration/PeakSet/docs/ci-cd-green.png)

Últimos runs: https://github.com/Sergtran/PeakSet/actions

---

Hecho con .NET, PostgreSQL y mucha disciplina. Cada serie cuenta. 💪
