# AI Assistant Onboarding Guide for the Blog Project

This guide helps AI assistants understand the project structure, architecture, and development workflows.

## Big Picture: Monorepo with .NET and React

This is a monorepo containing two main projects:

1.  **Backend:** An ASP.NET Core Web API located in the `src/` directory. It follows a clean architecture pattern.
2.  **Frontend:** A React application built with Vite, located in the `blogwebsite/` directory.

The two projects are designed to work together, with the React frontend consuming the .NET API.

### Backend Architecture (`src/`)

The backend is a .NET 9 solution (`Blog.sln`) structured with a clean architecture approach:

-   **`Blog.Api.WebApi`**: The main entry point of the API. This is the project you run to start the server. It handles HTTP requests and responses.
-   **`Blog.Api.Application`**: Contains the core business logic, services, and application features.
-   **`Blog.Api.Domain`**: Defines the core domain models and entities.
-   **`Blog.Infrastructure.Persistence`**: Implements data access logic, likely using Entity Framework Core.
-   **`Blog.Infrastructure.Security`**: Manages security aspects like authentication and authorization.

**Key Files:**
-   `Blog.sln`: The main solution file for the backend.
-   `src/Api/WebAPI/Blog.Api.WebApi/Program.cs`: The entry point for the backend application.
-   `src/Api/WebAPI/Blog.Api.WebApi/appsettings.Development.json`: Configuration for the development environment.

### Frontend Architecture (`blogwebsite/`)

The frontend is a standard React application set up with Vite.

-   **`src/main.jsx`**: The entry point for the React application.
-   **`src/App.jsx`**: The root component of the application.
-   **`src/components/`**: Contains reusable UI components (e.g., `Navbar.jsx`, `Footer.jsx`).
-   **`src/selections/`**: Contains larger page-level components or views.
-   **`src/admin/`**: Contains components and pages for the admin dashboard.
-   **`src/services/api.js`**: A dedicated module for making API calls to the backend.
-   **`.env`**: Contains environment variables, most importantly `VITE_API_BASE_URL`, which points to the backend API.

## Developer Workflows

### Running the Backend

To run the backend API, you can either open `Blog.sln` in Visual Studio and run the `Blog.Api.WebApi` project, or use the .NET CLI:

```bash
dotnet run --project src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj
```

The API will typically run on the port specified in `launchSettings.json` or `appsettings.Development.json`.

### Running the Frontend

To run the frontend development server:

1.  Navigate to the `blogwebsite` directory.
2.  Install dependencies: `npm install`
3.  Start the server: `npm run dev`

The frontend will connect to the backend API defined in `blogwebsite/.env`. Ensure the `VITE_API_BASE_URL` is correct.

````instructions
# AI Assistant Onboarding Guide for the Blog Project

This file contains focused, actionable guidance for AI coding agents to be immediately productive in this repository.

## Big picture (what matters)
- Monorepo with two primary apps: backend (.NET 9 solution under `src/`) and frontend (React + Vite under `blogwebsite/`).
- Backend follows a clean-architecture layout: `Blog.Api.WebApi` (HTTP entry), `Blog.Api.Application` (business logic), `Blog.Api.Domain` (entities/models), and `Blog.Infrastructure.*` (persistence, security, utilities).
- Frontend is a Vite React SPA. Communication with backend occurs through `blogwebsite/src/services/api.js` and `VITE_API_BASE_URL` in the frontend `.env`.

## Quick-start developer workflows (explicit commands)
- Build entire solution:
	- `dotnet build Blog.sln`
- Run backend (development):
	- `dotnet run --project src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj`
- Run frontend (development):
	- `cd blogwebsite; npm install; npm run dev`
- EF Core migrations (project-specific paths):
	- `dotnet ef migrations add <Name> --project src/Api/Infrastructure/Blog.Infrastructure.Persistence/Blog.Infrastructure.Persistence.csproj --startup-project src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj`
	- `dotnet ef database update --project src/Api/Infrastructure/Blog.Infrastructure.Persistence/Blog.Infrastructure.Persistence.csproj --startup-project src/Api/WebAPI/Blog.Api.WebApi/Blog.Api.WebApi.csproj`

## Important project-specific patterns & conventions
- Dependency-registration is modularized via extension methods called from `src/Api/WebAPI/Blog.Api.WebApi/Program.cs`. Look for `Add*Registration` methods across `Blog.Infrastructure.*` and `Blog.Api.Application`.
- Persistence:
	- `EntityContext` lives at `src/Api/Infrastructure/Blog.Infrastructure.Persistence/Context/EntityContext.cs` and centralizes DbSets and EF configuration.
	- The codebase uses interceptors (e.g., `AuditLogInterceptor`, `SavingChangesInterceptor`) added to the DbContext — check `EntityContext.OnConfiguring`.
	- Design-time factory pattern is used/expected for dotnet-ef tools: add `IDesignTimeDbContextFactory<EntityContext>` in the same persistence project to avoid hardcoded credentials.
- Configuration/secrets:
	- Connection strings should be provided via configuration keys found in `appsettings*.json` or environment variables. The repository uses the env-var naming convention `ConnectionStrings__ConnectionString` (note `__` for `:` in environment variables).
	- Frontend expects `VITE_API_BASE_URL` in `blogwebsite/.env`.
- Logs: Server logs (when configured) write to `src/Api/WebAPI/Blog.Api.WebApi/logs/` — check there for runtime issues.

## Integration points & cross-component data flows
- Frontend -> Backend: `blogwebsite/src/services/api.js` calls backend endpoints defined by controllers in `Blog.Api.WebApi`.
- Domain models defined in `Blog.Api.Domain` are mapped to persistence via configurations in `Blog.Infrastructure.Persistence/EntityConfigurations` and applied with `modelBuilder.ApplyConfigurationsFromAssembly(...)` in `EntityContext`.
- Authentication: JWT and security registration live in `Blog.Infrastructure.Security` and are wired into `Program.cs` via extension methods.

## Files to open first when debugging or implementing features
- `src/Api/WebAPI/Blog.Api.WebApi/Program.cs` — application startup and service registrations.
- `src/Api/Infrastructure/Blog.Infrastructure.Persistence/Context/EntityContext.cs` — DbSets, interceptors, OnConfiguring/OnModelCreating.
- `src/Api/Infrastructure/Blog.Infrastructure.Persistence/Extensions/Registration.cs` — AddDbContext wiring and configuration key names.
- `blogwebsite/src/services/api.js` and `blogwebsite/.env` — where frontend calls are defined and base URL configured.

## Examples and gotchas discovered in the codebase
- There was a hardcoded fallback connection string in `EntityContext.OnConfiguring` (search `Server=FSSARUHAN`), which is a security risk — prefer environment variables or `IDesignTimeDbContextFactory` for design-time.
- Migrations live in the persistence project under `src/Api/Infrastructure/Blog.Infrastructure.Persistence/Migrations/`. Logs have shown `PendingModelChangesWarning` in prior runs — ensure you create/apply migrations after model changes.
- Many registrations use extension methods (e.g., `AddApplicationRegistration`, `AddInfrastructureRegistration`); when adding new services follow the same pattern and keep scopes consistent (DbContext => scoped).

## Minimal checklist for common agent tasks
- Fix a backend bug that touches DB models:
	1. Update model/configuration in `Blog.Api.Domain` or `Blog.Infrastructure.Persistence/EntityConfigurations`.
 2. Add an EF migration using the dotnet-ef commands above (use design-time factory or set `ConnectionStrings__ConnectionString`).
 3. Run `dotnet ef database update` or generate SQL script for DB team.
- Expose a new API endpoint:
	1. Add controller under `src/Api/WebAPI/Blog.Api.WebApi/Controllers`.
 2. Add DTO/command in `Blog.Api.Application` and wire dependency registrations via the extension pattern.

## Limitations & where to ask for clarification
- Tests: there are few/no automated backend tests discoverable in the repo root; expect to run manual verification. If you need a testing convention, ask the repo owner.
- Secrets/CI: the repo assumes local environment variables or user-secrets for development. For CI or production, ask where secrets are stored (Key Vault or environment).

If anything here is unclear or you want this translated/localized, tell me which sections to expand or clarify.

````
