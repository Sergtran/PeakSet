# Rediseño Fase 5 — "Active Routine + Routine Statistics" (sin ciclos)

**Fecha:** 2026-09-03 (v2, decisión final tras conversaciones de producto)

## Decisión

**Se eliminan `TrainingCycle` y `CompletedTrainingCycle` del dominio.** No existe feature de ciclos.

El centro de la app es **"Mi rutina actual"** y PeakSet **calcula automáticamente** cuánto llevas usándola, cuántos entrenamientos has hecho, qué tan consistente has sido y cómo has progresado. Cero "iniciar ciclo", cero "avanzar semana", cero configuraciones.

Justificación:
- `activeCycle`/`previousCycle` eran de index.html; V2 es un rediseño (ADR-027), no un port.
- El concepto "ciclo" no aporta una decisión que el usuario deba tomar: la persona entrena con su rutina, ve "4 semanas · 12 entrenamientos", y cambia cuando quiere.
- Las estadísticas derivadas de `Routine + Workout` dan más valor y dejan un backend más interesante (consultas, agregaciones, fechas, LINQ/SQL, índices) — alineado con el objetivo de empleabilidad.

---

## Modelo final (más limpio)

```text
Routine
   ├── Sessions
   │      └── Exercises
   └── (Workouts referencian RoutineId? + snapshot de nombres)

Workout (fuente de verdad del uso)
   ├── WorkoutExercises  (name, type, laterality, PrStatus)
   └── WorkoutSets       (setNumber, reps, weight)
```

De ahí se **deriva** todo (sin tablas nuevas de estado):

| Métrica | Derivación |
|---|---|
| Rutina actual (una por usuario) | `UserSettings.CurrentRoutineId` (Guid?, FK RESTRICT) |
| Total entrenamientos | `COUNT(Workout)` con `RoutineId` de la rutina |
| Primera / última vez | `MIN/MAX(WorkoutDate)` |
| Semanas en uso | Días entre primera vez y hoy/último, en semanas |
| Últimos 7 días / días sin entrenar | Fechas de workouts recientes |
| Consistencia | Workouts agrupados por semana calendario (últimas N semanas) |
| PRs de la rutina | `COUNT(PrStatus == New)` sobre sus workouts |
| Línea de uso (historial) | Periodos derivados por huecos de fechas entre workouts ("Jun 03 ─ Jul 07 ↓ Jul 21 ─ actual") |
| Ejercicios más usados | Frecuencia por `WorkoutExercise.Name` |
| Stats por ejercicio (Fase 6) | Sesiones, series, PR (máx), primera/última vez, progreso |

**Regla de negocio clave:** "Cambiar de rutina" = actualizar `UserSettings.CurrentRoutineId`. El historial de cada rutina queda intacto porque los Workouts conservan su `RoutineId`/snapshot; el uso se re-deriva solo.

---

## Cambios sobre lo construido

1. **Domain:** borrar `TrainingCycle.cs` y `CompletedTrainingCycle.cs`. `UserSettings` gana `CurrentRoutineId` (Guid?, nullable = sin rutina actual).
2. **Tests:** eliminar `TrainingCycleTests` (casos de AdvanceWeek/Complete ya no existen). El resto (Routine/Workout/UserSettings) se mantiene; sumar tests del cambio en UserSettings si aplica.
3. **DbContext/Fluent:** quitar DbSets de `TrainingCycles`/`CompletedTrainingCycles` y sus configuraciones (CHECK CurrentWeek, UNIQUE UserId, índices); agregar configuración de `UserSettings.CurrentRoutineId` (FK RESTRICT a Routines).
4. **Migración:** eliminar las 2 tablas + agregar columna en `UserSettings` (dev sin datos; aviso de pérdida normal).
5. **Aplicación/API:** nueva feature "Active Routine + Stats" (ver API).
6. **Documentación:** ADR-031 (eliminar ciclos; rutina actual en UserSettings; stats derivadas) — supera a ADR-007. Actualizar docx (tabla de entidades, diccionario de datos, Fase 5/6, 1.2/1.3) y plan.

---

## API de la Fase 5 (rediseñada)

| Endpoint | Propósito |
|---|---|
| `PUT /api/users/current-routine` `{ routineId \| null }` | Elegir/cambiar (o quitar) la rutina actual |
| `GET /api/home` | Pantalla principal: rutina actual + stats de un vistazo (semanas, entrenamientos, último, días sin entrenar, PRs) |
| `GET /api/routines/{id}/stats` | Stats de cualquier rutina (total, primera/última, consistencia semanal, PRs) |
| `GET /api/routines/{id}/usage` | Línea de uso: periodos derivados por gaps de fechas |
| `GET /api/routines/{id}/exercises/top` | Ejercicios más realizados (frecuencia) |

Ejercicio por ejercicio (sesiones, series, PR, progreso) → Fase 6 (Stats + Calendario).

Esto es donde practicamos **SQL/LINQ de verdad**: `COUNT`, `MIN/MAX`, `GROUP BY` por semana, `COUNTIF` de PrStatus, partición por huecos de fechas, con los índices de Workouts `(UserId, WorkoutDate DESC)` ya creados.

---

## Orden de implementación

1. Domain: borrar ciclos; `UserSettings.CurrentRoutineId` (+ regla de una sola rutina actual).
2. DbContext + migración (drop 2 tablas, nueva columna) + actualizar tests.
3. Application: `ICurrentRoutineService` (cambiar/obtener) + `IRoutineStatsService` (home/stats/usage/top) + DTOs/validators.
4. Infrastructure: repos + queries agregadas sobre Workouts.
5. Api: `UsersController` (current-routine), `HomeController`, endpoints de stats por rutina.
6. Smoke test E2E: crear 2 rutinas → elegir actual → loguear workouts en fechas distintas → verificar stats, consistencia y línea de uso → cambiar de rutina → stats siguen por rutina.
7. Documentación: ADR-031 + actualizar docx y plan.

---

*Decisión cerrada en conversación (2026-09-03). Reemplaza por completo la versión v1 de este documento (que aún contemplaba ciclos).*
