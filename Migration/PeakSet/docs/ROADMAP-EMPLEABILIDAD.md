# Roadmap de empleabilidad C#/.NET — 12 semanas (3 meses)

**Fecha:** 2026-08-18
**Objetivo:** conseguir un empleo C#/.NET lo antes posible (máximo 12 semanas), usando PeakSet V2 como portafolio que cierra los gaps del CV.
**Regla de oro:** cada semana debe terminar con algo visible en el CV, GitHub o LinkedIn. No hay semanas de "solo estudiar".

---

## 1. Qué pide el mercado (17 ofertas analizadas, ago-2026)

Conteo sobre las ofertas a las que ya aplicaste (LinkedIn Colombia, remoto/híbrido):

| Keyword | % de ofertas | Nivel exigido típico |
|---|---|---|
| C# | 82% | Obligatorio |
| .NET / .NET Core | 82% | Obligatorio |
| SQL / SQL Server / T-SQL | 82% | Obligatorio |
| Arquitectura / system design | 71% | Mid/Senior |
| Cloud (Azure o AWS) | 71% | Mid/Senior |
| Testing (unit/integration) | 65% | Mid |
| ASP.NET / ASP.NET Core | 59% | Obligatorio |
| JavaScript | 59% | Mid |
| CI/CD (pipeline) | 59% | Mid/Senior |
| SQL Server (explícito) | 53% | Obligatorio |
| AWS | 53% | Mid/Senior |
| REST APIs | 53% | Obligatorio |
| Inglés requerido | 53% | B2+ |
| Azure | 47% | Mid |
| React | 41% | Mid (full stack) |
| Design Patterns | 41% | Mid |
| Docker | 35% | Mid |
| Entity Framework / EF Core | 35% | Obligatorio |
| Git | 29% (pero implícito en casi todas) | Básico |
| Microservicios | 24% | Senior |
| Stored Procedures | 24% | Mid/Senior |

**Lectura clave:** el mercado colombiano C#/.NET gira alrededor de **C# + .NET + SQL + ASP.NET Core + REST + Testing + Cloud (Azure/AWS) + CI/CD**. React aparece en 4 de cada 10 ofertas full stack. Microservicios, Kafka, Redis, Kubernetes aparecen solo en ofertas senior; NO son prioridad inicial.

### Niveles y salarios que se ven en las ofertas

| Nivel | Ejemplos | Salario visible |
|---|---|---|
| Junior (1-2 años) | Softtek Jr, Infotree | COP ~3-5M |
| Mid (3-4 años) | Perform, Insight Global, Solvo, Evalueserve | COP 5-8M; USD 1,733-2,773/mes (contractor) |
| Senior (4-7+ años) | SGS, AIM Edge, Ekos | COP 4.1-5M (SGS), COP 8M (AIM); USD 2,600-2,700 (Ekos) |

Con ~2.5-3 años de experiencia C# real (aunque en industria de juegos), el punto dulce es **Junior-Mid .NET y roles de L3 support/dev**, con los contratos en USD como objetivo de ascenso cuando el proyecto esté desplegado.

---

## 2. Dónde estás vs. dónde necesitas estar

| Skill | CV actual | PeakSet | Gap real | Prioridad |
|---|---|---|---|---|
| C# | ✅ Fuerte | ✅ Domain + 42 tests | Ninguno | Alta |
| OOP / SOLID / Patterns | ✅ Fuerte | ✅ Clean Architecture + DDD | Ninguno | Alta |
| ASP.NET Core / REST API | 🟡 En curso | 🔵 Fases 3-4 (controllers, DTOs) | Tener endpoints reales documentados | Alta |
| EF Core | 🟡 Base | 🔵 Fase 2 (DbContext + migraciones) | Migración + Fluent API funcionando | Alta |
| SQL / PostgreSQL | 🟡 Básico | 🔵 Fase 2 + queries de historial | Consultas, índices, joins reales | Alta |
| SQL Server / T-SQL | ❌ Ausente | 🔵 Postgres en el proyecto + práctica T-SQL | Ofertas lo piden 53% | Alta |
| Testing | 🟡 42 tests de dominio | 🔵 Tests de integración (Fase 3-4) | Tests sobre API + BD | Alta |
| JWT / Identity / Auth | 🟡 Menciona Authentication | 🔵 Fase 2 (Identity + Google + JWT) | Login real protegido | Alta |
| Git / GitHub | ✅ Básico | ✅ Repo público | README profesional + commits limpios | Media |
| Docker | 🟡 Lo lista | 🔵 docker-compose API + PostgreSQL | Dockerfile + compose completo | Media |
| CI/CD | ❌ Ausente | 🔵 GitHub Actions (build+test+deploy) | Pipeline con badge | Media |
| Azure | ❌ Ausente | 🔵 Deploy App Service + Azure SQL | App viva en internet | Media |
| React / TypeScript | ❌ Ausente | 🔵 Frontend básico (opcional) | Consumir API con JWT | Media |
| Inglés | 🟡 B2 | - | Técnico + entrevistas | Media-Alta |
| Microservicios / Kafka / Redis | ❌ Ausente | 🔵 Opcional (Hangfire/Redis) | Solo si sobra tiempo | Baja |

**Conclusión del gap:** tu núcleo C# es sólido y es real. Lo que te está dejando por fuera de las ofertas es (1) SQL Server/T-SQL, (2) una API ASP.NET Core terminada y probada, (3) cloud + CI/CD demostrables, y (4) la forma de contarlo en el CV. Todo eso se resuelve terminando PeakSet y actualizando el CV en cada hito.

---

## 3. Estrategia: dos carriles en paralelo

**Carril A — Búsqueda de empleo (desde hoy):**
- 10-15 aplicaciones/semana, siempre adaptando la sección de skills al anuncio.
- Seguimiento activo: los recruiters de tus ofertas ya aplicadas (Softtek, Evalueserve, AIM Edge, SGS…) tienen nombre; escribe un mensaje corto a los que están "actively reviewing".
- No esperar a "estar listo": Junior-Mid se consigue con lo que ya tienes + el proyecto avanzando.
- Dejar el 100% de los viernes para entrevistas y práctica de inglés técnico.

**Carril B — PeakSet es el plan de estudio:**
- No se estudian cursos sueltos: cada fase del proyecto agrega las keywords al CV con evidencia real.
- Cada feature termina con smoke test (ADR-025) y commit en GitHub.

---

## 4. Roadmap semana a semana

| Semana | Foco PeakSet | Entregable visible para CV | Keywords que agrega |
|---|---|---|---|
| 1 | **Fase 2:** Docker Desktop + dotnet-ef + Identity + EF Core + PostgreSQL (pasos 1-6 del doc 9.2) | DbContext Identity + ApplicationUser + conexión Postgres levantada | Entity Framework Core, PostgreSQL, ASP.NET Identity |
| 2 | **Fase 2 (fin):** migración inicial, seed del catálogo, verificación tablas + Swagger. Push + README base | Migraciones aplicadas, 12+ tablas, seed, repo público documentado | Migrations, Fluent API, Swagger/OpenAPI |
| 3 | **Fase 3 — Feature Routine:** DTO + Validator + Repository + Command + Controller + middleware de errores + tests de integración | Endpoints CRUD reales con tests | REST API, DTOs, Dependency Injection, xUnit, Clean Architecture |
| 4 | **Fase 4 — Feature Workout:** registrar entrenamiento, historial, PR + paginación + queries SQL | Historial y PR funcionando contra la BD | SQL queries, transactions, indexes, LINQ |
| 5 | **Fase 5 — TrainingCycle** + Dockerizar la API (Dockerfile + compose API+Postgres) | `docker compose up` levanta API + BD | Docker, Docker Compose |
| 6 | **CI/CD + Deploy:** GitHub Actions (build + test + imagen Docker) + Azure App Service + Azure SQL | Pipeline verde con badge + app viva en internet | CI/CD, GitHub Actions, Azure App Service, Azure SQL |
| 7 | **Azure hardening:** Key Vault, App Insights, variables de entorno + caso de estudio en README | README profesional con arquitectura + deploy | Azure Key Vault, Application Insights, Observability |
| 8 | **React + TypeScript (básico):** login con JWT + una pantalla que consume la API | Frontend React conectado a la API | React, TypeScript, JWT |
| 9 | **Full stack completo + pulido:** demo/screenshots, arreglar detalles, video corto opcional | Portafolio presentable: repo + demo + case study | Full Stack |
| 10 | **Entrevistas técnicas (intensivo):** SQL (LeetCode SQL), C#, .NET, arquitectura + mock interviews | Listo para entrevistas reales | SQL Server/T-SQL, Stored Procedures |
| 11 | **Aplicación agresiva + referidos:** apuntar a las ofertas mid/contractor (Insight, Solvo, Perform) + mensajes a recruiters | Pipeline de entrevistas activo | - |
| 12 | **Balance y negociación:** evaluar ofertas, negociar salario, decidir | Oferta firmada o plan B claro | - |

**Opcional si vas adelantado (semanas 8-9):** Hangfire (background jobs), Redis (caching), Serilog. Aparecen en ofertas senior; suman si sobran horas.

---

## 5. Dónde aplicar (las 17 ofertas analizadas)

### Alta prioridad (encajan hoy con tu perfil)
| Empresa | Rol | Por qué |
|---|---|---|
| Softtek | Desarrollador .NET Junior | 1-2 años, C#, SQL Server, Git, HTML/CSS/JS. Tu perfil exacto. |
| Infotree | Desarrollador de .NET | 1-3 años, soporte L3 + .NET/C# + SQL Server. Jugable. |
| Evalueserve | .Net Developer | 3-6 años, ASP.NET MVC, EF, SQL Server, SSO. Aplica cuando Fase 2 esté lista. |
| Softtek | Soporte a aplicaciones | 3+ años, .NET + Java. Tu debugging/producción cuenta. |

### Media prioridad (necesitan React o cloud para brillar)
| Empresa | Rol | Qué te falta para llegar |
|---|---|---|
| Perform | Full Stack .NET | React/Angular + 3 años full stack. Con Fases 3-4 + React básico. |
| Insight Global | Software Engineer (.Net) | React + AWS + Docker/K8s. $1,733-2,773/mes. Con Fases 6-8. |
| Solvo Global | Software Engineer | C# + React + SQL Server + PostgreSQL + OAuth + Hangfire. Después de Fase 8. |
| AIM Edge | Senior .NET | 4-7 años + microservicios + AWS. Stretch para el final. |
| SGS | SW Engineer Senior GBS | 4-7 años + SQL + ASP.NET/Node. Stretch. |
| Autolab | Senior AI-Native | .NET/MAUI + React + Azure/AWS + uso intensivo de IA. Tu perfil con IA encaja conceptualmente. |

### Baja prioridad / descartar
| Empresa | Rol | Por qué |
|---|---|---|
| Stefanini | Desarrollador Backend | Es Java/Spring, no .NET. |
| Polaris | Back End Developer | Migración .NET → Java. Solo si quieres Java. |
| Ekos | Senior C# .NET | 6+ años + React + Node + AWS avanzado. Objetivo a mediano plazo. |
| SGS (VB.NET) | Senior VB | 7+ años de VB. No es tu camino. |
| Empresa Confidencial | Lowcode | Menos alineado; útil solo como respaldo. |
| RVS | Video Game Designer | No es dev; guardar como backup por tu background de juegos. |

---

## 6. Plan de aplicaciones y networking

- **Volumen:** 10-15 aplicaciones/semana (2-3/día hábil). Calidad > cantidad: adapta el título y las skills al anuncio.
- **CV por anuncio:** reordena Technical Skills para que las 5-6 keywords del anuncio aparezcan en los primeros 8 términos. Los ATS cuentan coincidencias.
- **Mensaje a recruiter (cuando la oferta diga "actively reviewing" o la persona esté visible):** 3-4 líneas, en el idioma de la oferta: "Hola [nombre], apliqué a [rol] y quería compartir mi repo con la API ASP.NET Core en la que vengo trabajando (link). Tengo X años con C# en producción y creo que encajo bien. ¿Tienes 5 minutos esta semana?"
- **GitHub como portafolio:** README con arquitectura, capturas, `docker compose up` para correrlo, link a la demo desplegada. Eso responde "¿puedes entregar software real?" mejor que cualquier certificado.
- **LinkedIn:** título "Software Developer | C# / .NET | ASP.NET Core" (no "Unity dev"), proyecto PeakSet fijado arriba, y post corto cuando la app quede desplegada.
- **Inglés:** 30-45 min/día de práctica oral técnica (explicar tu proyecto en inglés en voz alta). Varias ofertas requieren B2+ conversacional.
- **Salario:** referencia COP 4-6M para Junior-Mid local; USD 1,700+ para contractor. No aceptes la primera oferta por debajo de tu rango sin negociar al menos una vez.

---

## 7. Temario de entrevistas (semana 10-11)

- **SQL (lo más probable):** SELECT/JOIN/GROUP BY, subqueries, CTE, window functions, índices, execution plans, transacciones, stored procedures. Practicar en LeetCode SQL.
- **C#:** OOP, SOLID, LINQ, async/await, generics, collections, delegates/events, exceptions, DI, value vs reference, string immutability.
- **.NET/Web:** ciclo de vida de ASP.NET Core, middleware, DI lifetimes, REST vs gRPC, JWT, CORS, validación, caching, error handling, EF Core (tracking vs no-tracking, lazy vs eager).
- **Arquitectura:** Clean Architecture, capas, Repository/Service, patrones de diseño más comunes (Factory, Strategy, Observer), monolith-first.
- **Algoritmos (según empresa):** LeetCode Easy-Medium (arrays, strings, hash maps, two pointers, stacks).
- **Comportamental:** formato STAR con historias reales: debugging complejo en producción, refactor de código legacy, trabajo bajo presión, manejo de conflictos.
- **Inglés técnico:** práctica de entrevista en inglés con IA o compañero.

---

## 8. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Docker no está instalado (bloquea Fase 2) | Instalar Docker Desktop el día 1; es prerequisito. |
| Fase 2 toma más de 2 semanas | Si se atasca, continuar con endpoints en memoria/tests y volver. No parar el carril de aplicaciones. |
| Falta SQL Server en el CV | Estudiar T-SQL en paralelo (curso corto + LeetCode SQL) y listarlo honestamente como "SQL (PostgreSQL, T-SQL)" cuando haya base. |
| Entrevistas técnicas con SQL/algoritmos | Bloque fijo diario de 1h de SQL + 1h de algoritmos desde la semana 6. |
| React no alcanza a quedar listo | No es bloqueante: backend + cloud + tests cubren la mayoría de ofertas. |
| Quedarse sin ahorros | La meta es oferta antes de la semana 12: las aplicaciones arrancan hoy, no al final. Roles Junior (Softtek, Infotree) se resuelven con lo que ya tienes. |

---

## 9. Acciones inmediatas (primeros 3 días)

1. **Hoy:** instalar Docker Desktop + `dotnet tool install --global dotnet-ef`. Dejar corriendo el instalador.
2. **Hoy:** subir la documentación al repo (commit de lo avanzado en git, README con estado actual). El repo es tu carta de presentación.
3. **Mañana:** actualizar el CV con lo que YA es verdad: PostgreSQL + EF Core en skills, link a GitHub, y "Available immediately". Agregar testing (42 tests de dominio) y Docker (ya lo tienes en compose).
4. **Mañana:** responder/mensajear a los 2-3 recruiters de las ofertas activas (Softtek, Evalueserve, AIM Edge, SGS).
5. **Día 3:** arrancar Fase 2 paso a paso según la sección 9.2 del documento técnico. Meta: "AspNetUsers + 12 tablas de dominio creadas con una migración" antes del fin de la semana 2.

---

*Este roadmap convive con `PeakSet-V2-Documentacion.docx` (secciones 8 y 9.2): el proyecto técnico sigue su orden; este documento decide qué se hace primero según el mercado.*
