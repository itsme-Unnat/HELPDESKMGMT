# HelpDeskManagement

Help Desk Ticket Management System built using ASP.NET Core Web API, ASP.NET Core MVC,
Entity Framework Core, SQL Server, xUnit, Moq and GitHub.

## Projects
- **HelpDesk.Api** — ASP.NET Core Web API. EF Core + SQL Server, Repository Pattern, TicketController.
- **HelpDesk.Mvc** — ASP.NET Core MVC app. Consumes the Web API via a Service Layer (HttpClient). No direct DB access.
- **HelpDesk.Tests** — xUnit + Moq unit tests for TicketController (repository mocked, no SQL Server dependency).

## Setup

### 1. HelpDesk.Api
1. Update the connection string in `HelpDesk.Api/appsettings.json` to point to your SQL Server instance.
2. Open a terminal in `HelpDesk.Api` and run:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   (Install the EF tool first if needed: `dotnet tool install --global dotnet-ef`)
3. Run the API: `dotnet run` — note the port shown (e.g. `https://localhost:7100`), Swagger UI is at `/swagger`.

### 2. HelpDesk.Mvc
1. Update `ApiBaseUrl` in `HelpDesk.Mvc/appsettings.json` to match the port the API is running on.
2. Run: `dotnet run`

### 3. HelpDesk.Tests
Run: `dotnet test`

## API Endpoints (HelpDesk.Api)
| Method | URL | Description |
|---|---|---|
| GET | /api/Ticket/All | Get all tickets |
| GET | /api/Ticket/{id} | Get ticket by Id |
| POST | /api/Ticket | Create a new ticket |
| PUT | /api/Ticket/{id} | Update an existing ticket |
| DELETE | /api/Ticket/{id} | Delete a ticket |
| GET | /api/Ticket/Status/{status} | Get all tickets by status |
