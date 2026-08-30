## EF Migrations
dotnet ef migrations add InitialCreate --project src/Infrastructure/Infrastructure.csproj --startup-project src/Library.Api/Library.Api.csproj --output-dir Persistence/Migrations

dotnet ef database update --project src/Infrastructure/Infrastructure.csproj --startup-project src/Library.Api/Library.Api.csproj

### Just in case: 
    dotnet ef migrations remove --project src/Infrastructure/Infrastructure.csproj --startup-project src/Library.Api/Library.Api.csproj


## Docker
- docker compose build api
- docker compose up -d
- docker compose down