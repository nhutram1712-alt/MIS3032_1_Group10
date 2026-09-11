# Backend — Smart Maintenance Facility Management

## Stack
- ASP.NET Core 9 Web API
- EF Core + SQL Server (LocalDB) / InMemory for Development & tests
- JWT + RBAC
- Serilog

## Run
```bash
cd src/backend/SmartMaintenance.Api
dotnet run
```

Development defaults to InMemory DB (`appsettings.Development.json`).
For SQL Server LocalDB, set `Database:Provider=SqlServer` and configure `ConnectionStrings:DefaultConnection`.
If LocalDB was created before EF migrations existed, drop `SmartMaintenanceDb` once so `InitialCreate` can apply.

```bash
cd src/backend
dotnet tool restore
dotnet ef database update --project SmartMaintenance.Infrastructure --startup-project SmartMaintenance.Api
```

## Demo users (seed)
| Username | Password | Role |
|---|---|---|
| manager | Due@2026 | FacilityManager |
| requester | Due@2026 | Requester |
| tech1 | Due@2026 | Technician |
| admin | Due@2026 | Admin |

## Test
```bash
cd src/backend
dotnet test
```

## Story coverage
- US-02-01: `POST /api/assets` (FacilityManager)
- US-03-01: `POST /api/requests` (Requester)
- US-04-01: `POST /api/work-orders` (FacilityManager)
- US-05-03: `POST /api/iot/ingest` (Gateway API Key), `GET /api/assets/{id}/iot-data` (FacilityManager)
- US-06-01: `GET /api/assets/{id}/prediction` (FacilityManager, Technician); background job optional via `Ai:BackgroundJobEnabled`

IoT ingest header: `X-Api-Key` (`Iot:GatewayApiKey`).
AI service: Python FastAPI in `src/ai` (`Ai:BaseUrl`). AI never creates Work Orders or changes Asset Status.
