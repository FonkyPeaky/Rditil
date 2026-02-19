# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

RDITIL is a WPF desktop examination/quiz platform for ITIL certification practice. Built with .NET 8.0, it uses MVVM architecture with Entity Framework Core and PostgreSQL.

## Build and Run Commands

```bash
# Restore dependencies
dotnet restore Rditil/Rditil.csproj

# Build the project
dotnet build Rditil/Rditil.csproj

# Run the application
dotnet run --project Rditil/Rditil.csproj

# EF Core migrations (from Rditil/ directory)
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Architecture

### MVVM + Service-Oriented Pattern

**Dependency Injection** is configured in `App.xaml.cs`:
- **Singletons**: `INavigationService`, `IAppState`, `IUserService`, `IAuditLogger`, `ISmtpSettingsProvider`, `IEmailService`
- **Transients**: All ViewModels, Views, and Windows

**Application Startup Flow**:
```
App.xaml.cs → LoginWindow (modal dialog) → MainWindow + WelcomeViewModel
                                        → ExamViewModel (40 questions, 1h timer)
                                        → EndPageViewModel (results + email report)
```

### Navigation System

`NavigationService` manages Frame-based page navigation using `ViewModelPageMapper` to resolve ViewModel types to Page types. ViewModels implement `INavigable` to receive `OnNavigatedTo()` callbacks with optional parameters.

### Key ViewModel-Page Mappings

| ViewModel | Page |
|-----------|------|
| LoginViewModel | LoginPage |
| WelcomeViewModel | WelcomePage |
| ExamViewModel | ExamenView |
| EndPageViewModel | EndPage |
| AdminPanelViewModel | AdminPanel |
| ProgressViewModel | ProgressPage |

### Database Layer

**Provider**: PostgreSQL via Npgsql
**Context**: `AppDbContext` in `Data/AppDbContext.cs`
**Connection**: Configured in `appsettings.json` under `ConnectionStrings:DefaultConnection`

**Main Tables**:
- `utilisateurs` - Users with BCrypt password hashes
- `questions` / `reponses` - Question bank with multiple-choice answers
- `exam_attempts` / `exam_answers` - Exam session tracking and individual responses
- `examens` - Legacy exam session table
- `smtp_settings` - Database-driven SMTP configuration

### Global State

`AppState` (singleton implementing `IAppState`) stores:
- `CurrentUser` - Logged-in user
- `ManagerEmail` - N+1 manager email for results
- `LastExamRows` / `LastExamResult` - Exam report data

## Key Services

| Service | Purpose |
|---------|---------|
| `UserService` | User CRUD via `IDbContextFactory<AppDbContext>` |
| `EmailService` | HTML exam reports via MailKit (dual config: DB + JSON fallback) |
| `NavigationService` | Frame navigation with parameter injection |
| `FileAuditLogger` | Writes to `%APPDATA%\Rditil\audit.log` |
| `PasswordHelper` | BCrypt hashing (work factor 12) |

## Configuration

`appsettings.json` contains:
- `ConnectionStrings:DefaultConnection` - PostgreSQL connection
- `Admin:Pin`, `MaxAttempts`, `LockoutSeconds` - Admin panel protection
- `Smtp:*` - Email server settings (fallback when DB settings unavailable)

## Conventions

- **Language mix**: French variable/table names, English code structure
- **Email domain**: Hardcoded `@randstaddigital.lu` in user creation
- **Exam flow**: 40 random questions, 1-hour countdown timer, auto-submit on timeout
- **Async pattern**: Uses `IDbContextFactory<AppDbContext>` for async database operations
- **CommunityToolkit.Mvvm**: `[ObservableProperty]` and `[RelayCommand]` source generators in ViewModels
