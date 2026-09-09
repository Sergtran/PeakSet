# PLAN INTEGRADO — Empleabilidad C#/.NET + PeakSet

**Documento de trabajo único** para adaptar PeakSet a las habilidades que pide el mercado y conseguir empleo en ≤12 semanas.

**Actualizado:** 2026-08-18
**Estado de implementación (2026-09-08):** Fases 1-6 completadas (Domain, Identity/EF Core/PostgreSQL, Auth + Feature Routine, Workout + historial + PR, Active Routine + Stats sin ciclos, Stats + Calendario + Import/Export). Siguiente: Fase 7 — Docker + CI/CD + Deploy (semana 5-6 del plan).

## Jerarquía de archivos (cómo se usan juntos)

| Archivo | Rol |
|---|---|
| **PLAN-INTEGRADO-EMPLEABILIDAD.md** (este) | Plan maestro operativo: qué construir, en qué semana, y qué agrega al CV. Se trabaja desde aquí. |
| **ROADMAP-EMPLEABILIDAD.md** | Análisis de mercado (17 ofertas), gaps del CV, ofertas objetivo, temario de entrevistas, plan de aplicaciones. |
| **PeakSet-V2-Documentacion.docx** | Referencia técnica: arquitectura, ADRs 001-027, diseño de BD, fases 1-7 y orden exacto de la Fase 2 (sección 9.2). |

---

## 1. Mapa: habilidad del mercado → fase de PeakSet → semana → CV

| Semana | Fase PeakSet (doc) | Habilidad del mercado | Keywords que agrega al CV | Entregable visible |
|---|---|---|---|---|
| 1-2 | Fase 2 | EF Core, PostgreSQL, Identity/JWT | Entity Framework Core, PostgreSQL, ASP.NET Identity, Migrations, Swagger | DbContext + tablas + seed + API levantando |
| 3 | Fase 3 — Feature Routine | REST API, DI, Testing | REST API, DTOs, Dependency Injection, xUnit, Clean Architecture | CRUD de rutinas con tests de integración |
| 4 | Fase 4 — Feature Workout | SQL, consultas | SQL, LINQ, Transactions, Indexes | Historial + PR con paginación |
| 5 | Fase 5 — TrainingCycle + Docker | Docker | Docker, Docker Compose | `docker compose up` levanta API + PostgreSQL |
| 6 | Fase 6 — CI/CD + Deploy | GitHub Actions, Azure | CI/CD, GitHub Actions, Azure App Service, Azure SQL | Pipeline verde + app viva en internet |
| 7 | Fase 7 — Hardening + README | Azure, observabilidad | Azure Key Vault, Application Insights | Caso de estudio completo en el repo |
| 8-9 | Fase 8 — Frontend | React, TypeScript, JWT | React, TypeScript, JWT | Login + pantalla consumiendo la API |
| 10-11 | Entrevistas | SQL Server/T-SQL, algoritmos, inglés | SQL Server, Stored Procedures | Listo para entrevistas + mocks |
| 12 | Negociación | - | - | Oferta firmada o plan B |

---

## 2. Adaptación de PeakSet por fase (qué se agrega al proyecto)

### Fase 2 — Identity + EF Core + PostgreSQL (semanas 1-2)
Ejecutar los 9 pasos de la sección 9.2 del documento técnico:
1. PostgreSQL con `docker compose up` (imagen postgres:16-alpine).
2. Instalar `dotnet-ef` global.
3. Paquetes en Infrastructure: `Npgsql.EntityFrameworkCore.PostgreSQL` y `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.
4. `ApplicationUser : IdentityUser` en Infrastructure/Identity.
5. `PeakSetDbContext : IdentityDbContext<ApplicationUser>` con Fluent API (value converters de Name/Repetitions/Weight, enums, índices únicos, CHECKs, FK CASCADE/RESTRICT).
6. Registrar DbContext + Identity en la DI (Program.cs del API).
7. Migración inicial: `dotnet ef migrations add InitialCreate` + `dotnet ef database update`.
8. Seed del catálogo DEFAULT_EXERCISES + UserSettings por defecto.
9. Verificación: tablas creadas (AspNetUsers + 12 de dominio), seed presente, API levanta, Swagger carga.

**Referencias:** ADR-021 (Identity desde el inicio), ADR-022 (Identity + Google + JWT), ADR-023 (PostgreSQL vía Docker).

### Fase 3 — Feature Routine (semana 3)
Vertical slice completo (ADR-024, ADR-026): DTO + Validator + Repository + Command/Service + Controller + middleware de errores + tests de integración + Swagger. Cerrar con smoke test usando index.html contra la API real (ADR-025).

### Fase 4 — Feature Workout (semana 4)
Registrar entrenamiento (series, peso, repeticiones), historial con paginación, cálculo de PR. Aquí se practican queries SQL reales, transacciones e índices (Workouts: `(UserId, WorkoutDate DESC)`).

### Fase 5 — Active Routine + Routine Statistics + Docker (semana 5)
Rediseño de producto (2026-09-03, decisión final): **se eliminan los ciclos** (`TrainingCycle`/`CompletedTrainingCycle`). El centro es "Mi rutina actual" (una por usuario, en `UserSettings.CurrentRoutineId`) y **todo se deriva de Routine + Workout**: semanas en uso, total de entrenamientos, consistencia semanal, PRs, línea de uso por gaps de fechas. Esta fase practica SQL/LINQ de verdad (COUNT, MIN/MAX, GROUP BY por semana, partición por fechas) sobre los Workouts de la Fase 4. Detalle completo: **docs/REDISENO-TRAININGCYCLE.md**. Dockerizar la API: Dockerfile + extender docker-compose para levantar API + PostgreSQL juntos con variables de entorno documentadas.

### Fase 6 — CI/CD + Azure (semana 6)
GitHub Actions: build → tests → publicación. Deploy a Azure App Service + Azure SQL. La app queda accesible por URL.

### Fase 7 — Hardening + portafolio (semana 7)
Azure Key Vault para secretos, Application Insights para logs, configuración por entorno. README profesional: arquitectura (Arquitectura general.png), capturas, instrucciones `docker compose up`, link a la demo.

### Fase 8-9 — React + TypeScript (semanas 8-9)
Frontend básico conectado a la API: login con JWT + una pantalla completa (ej: rutinas). No es necesario dominar React; basta con "construyo y consumo un frontend conectado a mi API".

### Opcional si vas adelantado
Hangfire (background jobs), Redis (caching), Serilog (logging). Son keywords de ofertas senior; suman solo si sobran horas.

---

## 3. Plan semanal combinado (técnico + búsqueda de empleo)

Distribución diaria sugerida: **mañana 4-5 h técnicas** (PeakSet) + **tarde 2 h búsqueda** (aplicaciones, mensajes a recruiters, inglés).

| Semana | Técnico (PeakSet) | Búsqueda de empleo | Hito CV |
|---|---|---|---|
| 1 | Instalar Docker + dotnet-ef; Identity + DbContext + conexión | CV actualizado (PostgreSQL, EF Core, GitHub); 10 aplicaciones | Repo con estado del proyecto documentado |
| 2 | Migración inicial + seed + verificación Swagger | Mensajes a recruiters de ofertas activas | "Entity Framework Core, PostgreSQL, ASP.NET Identity" |
| 3 | Feature Routine + tests de integración | 10-15 aplicaciones adaptadas | "REST API, xUnit, Dependency Injection" |
| 4 | Feature Workout + queries + PR | Práctica SQL 30 min/día | "SQL, LINQ, Transactions" |
| 5 | TrainingCycle + Docker completo | Práctica SQL + inglés 30 min/día | "Docker, Docker Compose" |
| 6 | GitHub Actions + Azure App Service | Post en LinkedIn del deploy | "CI/CD, GitHub Actions, Azure" |
| 7 | Key Vault + App Insights + README | Aplicar a ofertas mid (Perform, Insight, Solvo) | "Azure, Application Insights" |
| 8-9 | React + TypeScript + JWT | 15 aplicaciones/semana + mock interviews | "React, TypeScript, Full Stack" |
| 10 | Intensivo SQL + C# + .NET + algoritmos | 2-3 mock interviews/semana | "SQL Server, Stored Procedures" |
| 11 | Repaso de arquitectura + STAR + inglés | Aplicación agresiva + referidos | Pipeline de entrevistas activo |
| 12 | Evaluación de ofertas + negociación | Decidir | Oferta firmada |

---

## 4. Definición de "hecho" (Definition of Done por hito)

- **Fase 2 hecha:** `docker compose up` → API levanta, Swagger carga, tablas creadas, seed presente, tests verdes.
- **Cada feature hecha:** endpoints documentados en Swagger, tests de integración verdes, smoke test con index.html, commit + push a GitHub, keyword agregada al CV.
- **Semana cerrada:** hubo resultado visible (commit, deploy, aplicación enviada o mensaje a recruiter). No se cierra una semana solo con estudio.

---

## 5. Reglas de oro

1. **No estudiar sin construir:** cada tema termina en un commit de PeakSet.
2. **Nunca más de una semana sin resultado visible** (ADR-024: vertical slices).
3. **Aplicar desde hoy:** 10-15 aplicaciones/semana en paralelo; no esperar a "estar listo".
4. **CV actualizado solo con lo verificado:** no agregar keywords que el código no demuestre todavía.
5. **Unity se queda como diferenciador** en proyectos/entrevistas (multiplayer, estado distribuido, debugging de producción), pero el perfil es C#/.NET backend.

---

*Este plan es el pegamento entre ROADMAP-EMPLEABILIDAD.md (mercado y estrategia) y PeakSet-V2-Documentacion.docx (referencia técnica). Cuando cambie algo en uno, actualizar este archivo.*
