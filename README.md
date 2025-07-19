# 🧩 UVSystem

UVSystem is a **modular, production-ready backend system** built with **ASP.NET Core 9**, **Domain-Driven Design (DDD)**, and **Modular Monolith Architecture**.  
It integrates key modern software engineering practices like authentication with **Keycloak**, **PostgreSQL** for persistent storage, **distributed caching**, and **reliable messaging** through the **Inbox/Outbox** pattern.

![Architecture Diagram Placeholder](./docs/images/architecture-diagram.png)

---

## 📁 Project Structure

```
UVSystem/
├── src/
│   ├── Api/               # Presentation layer (Controllers, Middlewares)
│   ├── Common/            # Shared contracts, helpers, extensions
│   └── Modules/           # Business subdomains (Users, Roles, etc.)
├── docker-compose.yml     # 🔗 Main entry point (see below)
├── UVS.sln                # Solution file
└── .github/               # CI/CD, GitHub workflows
```

---

## 🚀 Getting Started

### 🐳 Run with Docker Compose (Recommended)

> This is the **main entry point** to run the entire backend system locally.

🔗 [Download `docker-compose.yml`](./docker-compose.yml) or use:

```bash
docker compose up --build
```

- Brings up:
  - ASP.NET Core API
  - Keycloak Identity Server
  - PostgreSQL Database
  - Distributed Cache (e.g., Redis - optional)
  - Message broker (e.g., RabbitMQ - optional)

---

## 🛠️ Manual Setup

### Prerequisites
- .NET 9 SDK
- Docker
- Postman
- Optional: Redis, RabbitMQ

### Clone & Run
```bash
git clone https://github.com/YOUR_USERNAME/UVSystem.git
cd UVSystem
dotnet run --project src/Api
```

---

## 🔐 Keycloak Integration (AuthN + AuthZ)

UVSystem uses [Keycloak](https://www.keycloak.org) for identity and access management.

### Setup Instructions

```bash
docker run -p 8080:8080 quay.io/keycloak/keycloak:24.0.1 start-dev
```

1. Open: `http://localhost:8080`
2. Login as `admin`
3. Create:
   - Realm: `uvs`
   - Confidential Client: `uvs.api` (service account enabled)
   - Roles: `Admin`, `User`, etc.
4. Update `appsettings.json`:

```json
"Keycloak": {
  "Authority": "http://localhost:8080/realms/uvs",
  "ClientId": "uvs.api",
  "ClientSecret": "YOUR_SECRET",
  "Audience": "account"
}
```

---

## 🧠 Core Architectural Concepts

### ✅ Domain-Driven Design (DDD)

- Code is organized by **subdomain** inside `Modules/`
- Each module owns:
  - Entities
  - Aggregates
  - Value Objects
  - Domain Services
  - Domain Events
  - Interfaces (e.g., `IUserRepository`)

### ✅ Clean Architecture / Onion Layers

- **Domain** layer is the core — contains logic
- **Application** defines use cases and interfaces
- **Infrastructure** implements those interfaces
- **Presentation** handles HTTP requests (Controllers, Middlewares)

---

## ✨ Features Explained

### 🔄 Caching

- Response caching and distributed caching (e.g., Redis)
- Speeds up frequent queries
- Configurable per module
- Sample in-memory caching also included

### 🐘 PostgreSQL

- Primary data persistence layer
- Integrated via `Npgsql.EntityFrameworkCore.PostgreSQL`
- Migrations are applied per module using `dotnet ef`

### 🔐 Identity & Roles

- All users are managed in Keycloak
- Roles like `Admin`, `User` are mapped to Keycloak roles
- JWT token validation handled via `Microsoft.AspNetCore.Authentication.JwtBearer`

### 📣 Domain Events

- Events raised **within** the same domain (e.g., `UserRegisteredDomainEvent`)
- Handled in-process via MediatR
- Enables loose coupling between aggregates and services

### 🌍 Integration Events

- Events raised **across** module boundaries or external systems (e.g., `UserCreatedIntegrationEvent`)
- Delivered via Outbox pattern (stored then dispatched)

### 📥 Inbox / 📤 Outbox Pattern

- Ensures reliable messaging even if consumers crash
- Outbox:
  - Events are stored in the DB then published
- Inbox:
  - Prevents double processing of received messages

### 🔄 Consumers

- Hosted services that subscribe to messages (via RabbitMQ, Kafka, etc.)
- Used to handle:
  - External sync
  - Email notifications
  - Background workflows

---

## 🔧 Technologies Used

| Area           | Tech                                                  |
|----------------|--------------------------------------------------------|
| Backend        | ASP.NET Core 9, C#                                     |
| Auth           | Keycloak (OpenID Connect)                              |
| Database       | PostgreSQL via EF Core                                 |
| Messaging      | MediatR, Background Services, Outbox                   |
| Caching        | MemoryCache, Redis (optional)                          |
| Docs           | Swagger, OpenAPI                                       |
| Patterns       | DDD, CQRS, Clean Arch, Event-driven                    |

---

## 🧪 Testing the API

### 1. Obtain Access Token via Postman

```http
POST http://localhost:8080/realms/uvs/protocol/openid-connect/token
```

**Body (x-www-form-urlencoded):**
```
client_id=uvs.api
client_secret=YOUR_SECRET
grant_type=client_credentials
```

### 2. Call API

```http
GET http://localhost:5000/api/users
Authorization: Bearer YOUR_ACCESS_TOKEN
```

---

## 📚 Learning Goals

This project is intended as a **reference implementation** for:

- Structuring enterprise-grade backends
- Applying DDD in a modular monolith
- Securing APIs with identity providers
- Applying messaging reliability patterns
- Separating concerns using clean architecture

---

## 📄 License

This project is licensed under the **MIT License**.
