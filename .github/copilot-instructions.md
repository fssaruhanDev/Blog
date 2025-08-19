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

## Project-Specific Conventions

-   **API Communication**: All frontend-to-backend communication should go through the service layer defined in `blogwebsite/src/services/api.js`.
-   **Styling**: The project uses a mix of global CSS (`App.css`, `index.css`) and component-specific CSS files (e.g., `Navbar.css`).
-   **Routing**: Frontend routing is likely handled by a library like React Router. The route definitions can be found in `blogwebsite/src/constants/routes.js`.
-   **State Management**: Simple state is managed with React hooks. For more complex state, a state management library might be used.
