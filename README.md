# Flight  API

Flight  Management API built with .NET 8, following Domain-Driven Design (DDD) principles and Clean Architecture patterns.

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Docker (for PostgreSQL and Redis)
- .NET Aspire 

### Running the Application

1. **Clone the repository**
   ```bash
   git clone 
   cd FlightStatus
   ```

2. **Start with .NET Aspire**
   ```bash
   cd FlightStatus.AppHost
   dotnet run
   ```

3. **Access the API**
   - API: `https://localhost:7001`
   - Swagger UI: `https://localhost:7001` (in development)

### Default Users

The database is automatically initialized with the following users:

| Username | Password | Role | Permissions |
|----------|----------|------|-------------|
| `user` | `password123` | User | Read flights only |
| `moderator` | `password123` | Moderator | Full CRUD operations |
| `admin` | `password123` | Moderator | Full CRUD operations |

### Sample Data

The application seeds with 8 sample flights covering major US routes with different statuses:
- ✅ In Time flights
- ⏰ Delayed flights  
- ❌ Cancelled flights

## 📡 API Endpoints

### Authentication
- `POST /api/auth/login` - Get JWT token

### Flights
- `GET /api/flights` - Get all flights (with filtering)
- `POST /api/flights` - Create flight (Moderator only)
- `PUT /api/flights/{id}/status` - Update flight status (Moderator only)

## 🏗️ Architecture

- **Domain Layer**: Entities, events, and business rules
- **Application Layer**: CQRS with MediatR, commands, and queries
- **Infrastructure Layer**: EF Core, Redis caching, and external services
- **Presentation Layer**: Web API controllers

## 🔧 Features

- ✅ JWT Authentication with role-based authorization
- ✅ Redis distributed caching with smart invalidation
- ✅ Domain events with EF Core interceptors
- ✅ Comprehensive logging with Serilog
- ✅ Input validation with FluentValidation
- ✅ BCrypt password hashing
- ✅ Swagger/OpenAPI documentation
- ✅ Unit and integration tests
- ✅ .NET Aspire orchestration

## 🧪 Testing

Run tests with:
```bash
dotnet test
```


