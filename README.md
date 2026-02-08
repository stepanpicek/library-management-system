# Library Management System

Systém pro správu knihovny postavený na .NET 10 s Blazor Server UI a REST API.

## Architektura

Projekt využívá **Clean Architecture** s jasným oddělením vrstev:

```
src/
├── LibraryManagementSystem.Domain        # Doménové entity a výjimky
├── LibraryManagementSystem.Application   # Business logika, CQRS commands/queries
├── LibraryManagementSystem.Infrastructure # Datová vrstva, EF Core, SQLite
└── LibraryManagementSystem.Web           # Prezentační vrstva, API, Blazor UI
```

### CQRS Pattern

Aplikace implementuje **CQRS (Command Query Responsibility Segregation)** pomocí MediatR:

- **Commands**: `CreateBook`, `BorrowBook`, `ReturnBook`
- **Queries**: `GetBooks`, `GetBookDetail`

### MediatR Pipeline

Požadavky procházejí pipeline behaviours:

1. `LoggingBehaviour` - logování požadavků
2. `ValidationBehaviour` - validace pomocí FluentValidation

## Použité technologie

| Kategorie | Technologie |
|-----------|-------------|
| Framework | .NET 10, ASP.NET Core |
| UI | Blazor Server, Bootstrap 5 |
| API | REST API s OpenAPI/Swagger |
| ORM | Entity Framework Core 10 |
| Databáze | SQLite |
| Validace | FluentValidation |
| Mediator | MediatR |
| Testování | xUnit, WebApplicationFactory |

## Uživatelské rozhraní

Blazor Server aplikace poskytuje:

- **Seznam knih** (`/`) - tabulka všech knih s fulltextovým vyhledáváním
- **Detail knihy** (`/detail/{id}`) - informace o knize včetně historie výpůjček
- **Přidání knihy** (`/add`) - formulář pro vytvoření nové knihy s validací
- **Vypůjčení knihy** - snížení počtu dostupných kopií
- **Vrácení knihy** - zvýšení počtu dostupných kopií
- **Toast notifikace** - zpětná vazba při akcích (úspěch, chyba, varování)

## REST API

API je dostupné na `/api/books` a dokumentované přes Swagger UI.

### Endpointy

| Metoda | Endpoint | Popis |
|--------|----------|-------|
| `GET` | `/api/books` | Seznam knih s filtrováním a stránkováním |
| `PUT` | `/api/books/{id}/borrow` | Vypůjčení knihy |
| `PUT` | `/api/books/{id}/return` | Vrácení knihy |

### Příklady

```bash
# Seznam všech knih
curl http://localhost:5217/api/books

# Vyhledávání s filtrováním
curl "http://localhost:5217/api/books?text=Orwell&page=1&size=10"

# Vypůjčení knihy
curl -X PUT http://localhost:5217/api/books/{id}/borrow

# Vrácení knihy
curl -X PUT http://localhost:5217/api/books/{id}/return
```

## Spuštění aplikace

### Prerekvizity

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) nebo [Docker](https://www.docker.com/)

### Možnost 1: Spuštění pomocí .NET CLI

```bash
# Klonování repozitáře
git clone <repository-url>
cd library-management-system

# Obnovení závislostí
dotnet restore

# Spuštění aplikace
dotnet run --project src/LibraryManagementSystem.Web

# Aplikace běží na:
# - http://localhost:5217 (HTTP)
# - https://localhost:7125 (HTTPS)
```

### Možnost 2: Spuštění pomocí Dockeru

```bash
# Build Docker image
docker build -t library-management-system -f src/LibraryManagementSystem.Web/Dockerfile .

# Spuštění kontejneru
docker run -d -p 8080:8080 --name library-app library-management-system

# Aplikace běží na http://localhost:8080
```

### Swagger UI

Po spuštění je Swagger dokumentace dostupná na:
- http://localhost:5217/swagger (dotnet)
- http://localhost:8080/swagger (docker)

## Testování

```bash
# Spuštění všech testů
dotnet test

# Spuštění s detailním výstupem
dotnet test --verbosity normal
```

Projekt obsahuje integrační testy pokrývající:
- Získání seznamu knih s filtrováním a stránkováním
- Vypůjčení a vrácení knih
- Validace chybových stavů

## Struktura databáze

```
┌─────────────────┐       ┌─────────────────┐
│     Books       │       │     Loans       │
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │◀──────│ BookId (FK)     │
│ Title           │       │ Id (PK)         │
│ Author          │       │ BorrowedAt      │
│ Year            │       │ ReturnedAt      │
│ Isbn (unique)   │       └─────────────────┘
│ AvailableCopies │
└─────────────────┘
```

Relace: **Book 1:N Loan** - jedna kniha může mít více výpůjček (aktivních i vrácených).

## Seed data

Při prvním spuštění se automaticky vytvoří testovací data:

| Název | Autor | Rok | Dostupné kopie |
|-------|-------|-----|----------------|
| Malý princ | Antoine de Saint-Exupéry | 1943 | 9 |
| 1984 | George Orwell | 1949 | 0 |
| R.U.R. | Karel Čapek | 1920 | 5 |

## Využití AI

Při vývoji projektu byly využity následující AI nástroje:

| Nástroj | Model | Využití                                                                                             |
|---------|-------|-----------------------------------------------------------------------------------------------------|
| ChatGPT | GPT 5.2 | Analýza, kontrola myšlenek                                                                          |
| Claude Code | Opus 4.5 | Přidání Bootstrap stylů do UI, vygenerování integračních testů a Readme, code review, fixování chyb |