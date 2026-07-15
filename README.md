# Library Management System — .NET Minimal Web API

A backend API for a small library to manage books, members, and borrowing/returning workflows, built as a .NET 10 Minimal API with a clean layered architecture.

## Project Overview

The system supports:

- **Book management** — full CRUD, with ISBN uniqueness and copy-availability tracking
- **Member management** — registration and profile management, with active/inactive status
- **Borrowing workflow** — borrowing and returning books, with business rules around availability, member status, and borrowing limits

Architecture follows: `Endpoint → Service → Repository → DbContext`, keeping endpoints thin and business rules centralized in the Domain and Application layers.

## Technologies Used

- **.NET 10** — Minimal APIs
- **Entity Framework Core** — ORM, migrations
- **PostgreSQL** — database (via Npgsql provider)
- **Docker / Docker Compose** — local PostgreSQL instance
- **Swashbuckle (Swagger/OpenAPI)** — API documentation and testing UI
- **xUnit + NSubstitute** — unit testing
- Repository pattern + DTO contracts + DataAnnotations validation + global exception handling middleware

## Project Structure

```
Library.Api/                  # Solution root
  Library.Api/                # Main API project
    Domain/                   # Entities (Book, Member, Borrowing) and enums
    Application/               # Services, repository interfaces, custom exceptions
    Infrastructure/             # EF Core DbContext, repositories, entity configurations, migrations
    Contracts/                 # Request/response DTOs, grouped by feature
    Endpoints/                  # Minimal API route definitions
    Filters/                    # Validation filter
    Middleware/                 # Global exception handling
    docker-compose.yml
  Library.Api.Tests/           # Unit test project
  Library.Api.sln
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- `dotnet-ef` CLI tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Getting Started

### 1. Start PostgreSQL via Docker

From the `Library.Api/Library.Api` folder (where `docker-compose.yml` lives):

```bash
docker compose up -d
```

This starts a Postgres 16 container with:

- Database: `librarydb`
- User: `libraryuser`
- Password: `librarypassword`
- Port: `5432`

Verify it's running:

```bash
docker ps
```

### 2. Apply EF Core migrations

From the `Library.Api/Library.Api` folder:

```bash
dotnet ef database update
```

This creates all tables, indexes, and foreign keys, and seeds a small set of sample data (a few books, members, and one active borrowing record) via `IEntityTypeConfiguration.HasData()`.

> Migrations also apply automatically on application startup (see `MigrateDb()` in `Program.cs`), so this step is optional if you just want to run the API directly — but running it explicitly first lets you confirm the database connection works before starting the app.

### 3. Run the API

From the `Library.Api/Library.Api` folder:

```bash
dotnet run
```

The API will be available at:

- HTTP: `http://localhost:5244`
- HTTPS: `https://localhost:7089`

### 4. Access Swagger

Once running, open:

```
https://localhost:7089/swagger
```

All endpoints are documented and testable directly from the Swagger UI, grouped by tag (Books, Members, Borrowing).

## Running Tests

From the solution root (`Library.Api/`, the folder containing `Library.Api.sln`):

```bash
dotnet test
```

This runs the full test suite, covering the core borrowing business rules:

- A book cannot be borrowed when no copies are available
- An inactive member cannot borrow a book
- A member cannot borrow more than 3 active books
- Returning a book increases available copies
- A book cannot be returned twice

Tests are split by layer:

- `Library.Api.Tests/Domain/` — entity-level rules (`Book`, `Borrowing`), tested directly with no mocking
- `Library.Api.Tests/Application/Services/` — cross-entity rules (member status, borrowing limits), tested against `BorrowingService` with repository dependencies mocked via NSubstitute

## Working with Migrations

Migrations live in `Library.Api/Infrastructure/Data/Migrations`. Whenever an entity or entity configuration changes:

```bash
cd Library.Api/Library.Api
dotnet ef migrations add <DescriptiveName>
dotnet ef database update
```

Always inspect the generated migration file before applying it, particularly for renames or changes that could affect existing data.

To remove the most recently added (unapplied) migration:

```bash
dotnet ef migrations remove
```

## Example API Requests

**Create a book**

```http
POST /api/books
Content-Type: application/json

{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "publishedYear": 2008,
  "totalCopies": 3
}
```

**Register a member**

```http
POST /api/members
Content-Type: application/json

{
  "fullName": "Jane Doe",
  "email": "jane.doe@example.com",
  "phoneNumber": "0771234567"
}
```

**Borrow a book**

```http
POST /api/borrowings
Content-Type: application/json

{
  "bookId": "11111111-1111-1111-1111-111111111111",
  "memberId": "44444444-4444-4444-4444-444444444444"
}
```

**Return a book**

```http
POST /api/borrowings/{id}/return
```

**View a member's borrowing history**

```http
GET /api/members/{memberId}/borrowings
```

## Sample Data

On first run, the database is seeded with sample books, members, and one active borrowing record — no manual setup needed to start exploring the API via Swagger immediately after `docker compose up -d` and `dotnet ef database update`.

## Assumptions Made

- **ISBN is immutable once a book is created.** `UpdateBookRequest` does not include `Isbn` — it's treated as a stable business identifier, similar to a product SKU. Changing it would effectively represent a different book.
- **`PUT` endpoints follow full-replacement semantics**, per REST convention — clients are expected to resend the complete resource (all fields), not just the fields being changed. Partial updates (PATCH) were not implemented, as they weren't part of the specification.
- **`PhoneNumber` is optional** for members, since the specification's business rules only mark `FullName` and `Email` as required.
- **Borrowing status `Overdue`** is a defined state on the entity but is not automatically transitioned by a background process in this implementation — `MarkOverdueIfApplicable()` exists on the `Borrowing` entity but would need a scheduled job to run it in production. Automatic overdue detection was treated as bonus scope.
- **`appsettings.json` and `appsettings.Development.json` are committed to source control.** Ordinarily, `appsettings.Development.json` (and any file containing credentials) would be excluded via `.gitignore`, since it typically holds local secrets or environment-specific values that shouldn't be shared. In this project, the only values present are local Docker Compose credentials for a disposable local Postgres instance (not a real/shared/production database), so there's no meaningful security exposure in versioning them — doing so simply makes the project easier for a reviewer to clone and run immediately without extra setup steps. This would **not** be appropriate practice for a real production system, where connection strings, API keys, and secrets should always be kept out of version control (e.g. via user secrets, environment variables, or a secrets manager).
