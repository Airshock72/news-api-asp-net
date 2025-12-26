# Training Solution

This repository contains a small training solution with three related .NET projects that demonstrate Minimal APIs, MVC controllers, Entity Framework Core (SQLite), the repository pattern, and MediatR. This README explains the projects, how to run each one separately, the available endpoints, database/migrations, configuration, and troubleshooting steps.

Projects
- `NewsMinimalApi` — a minimal API (single-file Program.cs) that stores news items in-memory (no persistent DB). Good for learning Minimal APIs and basic routing.
- `NewsMVC` — an ASP.NET Core MVC / Web API style project using Entity Framework Core with SQLite (LocalDB.db). Demonstrates controllers, EF Core DbContext, migrations, and views.
- `NewsMVCRepository` — similar to `NewsMVC` but uses a repository pattern and MediatR handlers for requests; includes common response wrappers and repositories.

Repository layout (top-level)
- `Training.sln` — Visual Studio solution containing all projects.
- `NewsMinimalApi/` — Minimal API project
- `NewsMVC/` — MVC + EF Core project (uses `LocalDB.db` as SQLite file)
- `NewsMVCRepository/` — MVC + Repository + MediatR project (uses same SQLite DB file by default)

Requirements & assumptions
- .NET SDK installed (check with `dotnet --version`). The projects target a recent .NET version; use the SDK that matches or is newer than the target framework in the `.csproj` files.
- On Windows, PowerShell is used for command examples below.
- `dotnet-ef` may be needed for applying migrations (install with `dotnet tool install --global dotnet-ef`).

Quick global commands (PowerShell)

Build the entire solution:

```powershell
dotnet build .\Training.sln -c Debug
```

Run a single project (replace path with whichever project you want to run):

```powershell
# Run NewsMVC
dotnet run --project .\NewsMVC\NewsMVC.csproj

# Run NewsMVCRepository
dotnet run --project .\NewsMVCRepository\NewsMVCRepository.csproj

# Run NewsMinimalApi
dotnet run --project .\NewsMinimalApi\NewsMinimalApi.csproj
```

Set environment for Development (PowerShell example) and run:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Development'; dotnet run --project .\NewsMVC\NewsMVC.csproj
```

Per-project details

1) NewsMinimalApi

Purpose
- Minimal API sample that keeps all data in memory (List<News>). It's single-file (Program.cs) and useful to inspect Minimal API routing and DTOs quickly.

Important files
- `NewsMinimalApi/Program.cs` — contains routes, DTOs and models.
- `NewsMinimalApi/appsettings.json` — logging and hosts settings.

Build & run
- No DB setup needed. Run with:

```powershell
dotnet run --project .\NewsMinimalApi\NewsMinimalApi.csproj
```

Endpoints (all routes under root `/`)
- POST /News
  - Request body: { "Title": "...", "Content": "..." }
  - Returns: Guid (id of created news)
- GET /News/{id}
  - Returns: full news item (id, title, content, date, comments) or 404 with a Georgian message when not found.
- GET /News
  - Returns: list of news summary objects [{ Id, Title, Date }]
- DELETE /News/{id}
  - Deletes an in-memory item; returns 200 OK or 404
- PUT /News/{id}
  - Request body: { "Title": "...", "Content": "..." }
  - Updates an in-memory item; returns 200 OK or 404
- POST /News/{id}/Comment
  - Request body: { "Text": "..." }
  - Adds a comment to a news item; returns Guid of created comment or 404
- DELETE /News/{id}/Comment/{commentId}
  - Deletes a comment; returns 200 or 404

Notes
- Data is ephemeral; when the process restarts, data is lost.
- The Minimal API enables OpenAPI/Swagger endpoints during Development via `MapOpenApi()`.

2) NewsMVC

Purpose
- Demonstrates an MVC/Web API-style project using Entity Framework Core with SQLite.
- Stores news and news comments in a `LocalDB.db` SQLite file (by default) and exposes controllers under `/News`.

Important files
- `NewsMVC/Program.cs` — configures services, DbContext, and controllers.
- `NewsMVC/Controllers/NewsController.cs` — controller exposing CRUD endpoints and comment endpoints.
- `NewsMVC/Data/TrainingDataContext.cs` — EF Core DbContext with DbSets and relationships.
- `NewsMVC/appsettings.json` — contains the connection string: `"ConnectionStrings:SQLiteDefault": "Data Source=LocalDB.db"`.
- `NewsMVC/Migrations/` — EF Core migration files (already present in repo).
- `NewsMVC/LocalDB.db` — SQLite DB file (if present in the repository).

Build & run

```powershell
# Ensure migrations are applied (see migrations section). Then run:
dotnet run --project .\NewsMVC\NewsMVC.csproj
```

Endpoints (controller routes: `/News`)
- GET /News — returns all news summaries
- GET /News/{id} — returns full news with comments or 404
- POST /News — body: { "Title","Content" } -> creates news and returns new Guid
- DELETE /News/{id} — deletes a news item
- PUT /News/{id} — updates news (body: { "Title","Content" })
- POST /News/{id}/Comments — body: { "Text" } -> create comment for news
- DELETE /News/{id}/Comments/{commentId} — delete a comment

Database & migrations (NewsMVC)
- Default provider: SQLite using `LocalDB.db` (configured in `appsettings.json` under `ConnectionStrings:SQLiteDefault`).
- Migrations are provided in `NewsMVC/Migrations/`.

To apply migrations from the repo (PowerShell):

```powershell
# Install tooling if needed
dotnet tool install --global dotnet-ef

# From repo root, run EF update for the project that contains the DbContext
dotnet ef database update --project .\NewsMVC\ --startup-project .\NewsMVC\
```

Notes
- If the repository includes a `LocalDB.db` file already, the app can run without running migrations, but applying migrations is the correct way to ensure schema matches.
- If you change the schema or add migrations, re-run `dotnet ef database update`.

3) NewsMVCRepository

Purpose
- Similar to `NewsMVC` but organized with repositories and MediatR handlers. Controllers delegate to MediatR and repositories.
- Uses `NewsMVCRepository/Common/BaseResponse.cs` to standardize responses.

Important files
- `NewsMVCRepository/Program.cs` — configures DbContext, MediatR, and repositories.
- `NewsMVCRepository/Controllers/NewsController.cs` — controller that uses MediatR and repositories.
- `NewsMVCRepository/Repositories/` — repository classes for data access (Add/Get/Remove/Save methods).
- `NewsMVCRepository/Data/TrainingDataContext.cs` — EF Core DbContext.
- `NewsMVCRepository/appsettings.json` — connection string (same `Data Source=LocalDB.db` by default).

Build & run

```powershell
dotnet run --project .\NewsMVCRepository\NewsMVCRepository.csproj
```

Endpoints
- Same route surface as `NewsMVC` under `/News`, but commands/queries flow through MediatR and repositories.

Database & migrations (NewsMVCRepository)
- Uses SQLite `LocalDB.db` by default (see `appsettings.json`).
- Migrations (if present) will be in `NewsMVCRepository/Migrations/` and can be applied similarly:

```powershell
dotnet ef database update --project .\NewsMVCRepository\ --startup-project .\NewsMVCRepository\
```

Configuration, appsettings, and environment
- `appsettings.json` contains logging and `ConnectionStrings:SQLiteDefault` used by the two MVC projects.
- `appsettings.Development.json` can override settings for development.
- Environment variable `ASPNETCORE_ENVIRONMENT` controls usage of Development settings and whether OpenAPI endpoints are mapped in `Program.cs`.
- To change DB file location, update `ConnectionStrings:SQLiteDefault` in the respective project's `appsettings.json` or via environment variables.

Applying migrations and `dotnet-ef`
- If you don't have `dotnet-ef` installed, install it once:

```powershell
dotnet tool install --global dotnet-ef
```

- Example to add a migration (if modifying models):

```powershell
# From repo root, add migration to NewsMVC
dotnet ef migrations add AddSomeField -p .\NewsMVC\ -s .\NewsMVC\
```

Troubleshooting
- "SQLite busy/locked" — occurs when multiple processes access the same SQLite file. Close other apps or use a copy of the DB per process.
- Port already in use — change the port using `ASPNETCORE_URLS` or in `launchSettings.json`, or stop the conflicting process.
- Migration errors / startup exceptions — ensure EF Core tools are installed and the correct project is used with `--project` and `--startup-project`.
- HTTPS redirection issues during local testing — use the console output to see the listening URLs; you can disable HTTPS redirection in `Program.cs` if needed for testing.

Quick API testing examples (PowerShell Invoke-RestMethod)

```powershell
# Create a news item on an API running on http://localhost:5000
Invoke-RestMethod -Method Post -Uri http://localhost:5000/News -Body (@{ Title='Hello'; Content='World' } | ConvertTo-Json) -ContentType 'application/json'

# Get all news
Invoke-RestMethod -Method Get -Uri http://localhost:5000/News
```

Next steps and suggestions
- Add unit and integration tests for controllers, repositories, and MediatR handlers.
- Add Dockerfiles for containerized runs and a docker-compose to orchestrate multiple services.
- Add a CI pipeline for automated build, test, and publish.
- Consider separating shared models into a class library if multiple projects reuse DTOs.

Contact / Author notes
- This README was generated from the repository's Program.cs, Controllers, DbContext and appsettings files; if anything in your local checkout differs (launchSettings, environment overrides), prefer local settings.


---
Requirements coverage
- Per-project explanation: Done
- Build & run (PowerShell): Done
- Endpoints: Done (listed for each project)
- DB & migrations: Done (instructions to apply migrations)
- Environment/appsettings: Done
- Troubleshooting & next steps: Done

If you want, I can:
- Add example curl commands for each endpoint.
- Create a `scripts/` folder with PowerShell scripts to run, build, and run migrations.
- Add Dockerfiles and a docker-compose for each project.


