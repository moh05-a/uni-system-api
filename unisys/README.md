# 🎓 UniSys API – University Management System

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0%20%7C%209.0-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/PostgreSQL-Database-336791?style=for-the-badge&logo=postgresql" />
  <img src="https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge" />
  <img src="https://img.shields.io/badge/RabbitMQ-Message%20Broker-FF6600?style=for-the-badge&logo=rabbitmq" />
  <img src="https://img.shields.io/badge/License-MIT-success?style=for-the-badge" />
</p>

<p align="center">
A scalable and modular <strong>ASP.NET Core Web API</strong> for managing university data using
<strong>Clean Architecture</strong>, the <strong>Repository-Service Pattern</strong>, and <strong>RabbitMQ</strong> for asynchronous messaging.
</p>

---

# 📖 Overview

**UniSys API** is a backend system built with **ASP.NET Core** that demonstrates modern backend development practices.

The project manages university resources such as:

- 🎓 Students
- 👨‍🏫 Tutors
- 📚 Subjects

while following software engineering best practices including:

- Clean Architecture
- Repository Pattern
- Service Layer
- Dependency Injection
- Global Exception Handling
- Action Filters
- DTO Mapping
- RabbitMQ Messaging
- Entity Framework Core
- PostgreSQL

---

# ✨ Features

### 👨‍🎓 Student Management

- Create Students
- Retrieve Students
- Update Students
- Delete Students

### 👨‍🏫 Tutor Management

- CRUD Operations
- Clean Service Layer

### 📚 Subject Management

- CRUD Operations
- Entity Relationships

### 📨 RabbitMQ Integration

- Publish events when a student is created
- Background Consumer Service
- Topic Exchange
- Routing Keys
- Asynchronous Communication

Example event:

```json
{
  "id": 1,
  "name": "Mohammad",
  "major": "Computer Science",
  "createdAt": "2026-07-20T12:00:00"
}
```

Published using:

```
student.created
```

---

# 🏗️ Architecture

```
                Client
                   │
                   ▼
            ASP.NET API
                   │
         ┌─────────┴─────────┐
         ▼                   ▼
   Service Layer       RabbitMQ Publisher
         │                   │
         ▼                   ▼
 Repository Layer       Topic Exchange
         │                   │
         ▼                   ▼
   PostgreSQL DB       RabbitMQ Queue
                             │
                             ▼
                    Background Consumer
```

---

# 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core | REST API |
| C# | Programming Language |
| Entity Framework Core | ORM |
| PostgreSQL | Database |
| RabbitMQ | Message Broker |
| Dependency Injection | Service Registration |
| Action Filters | Request Logging |
| Middleware | Global Exception Handling |

---

# 📂 Project Structure

```
UniSys
│
├── Controllers
├── Services
├── Repositories
├── DTOs
├── Data
├── Messaging
│   ├── RabbitMqPublisher
│   ├── RabbitMqConsumerService
│   └── IMessagePublisher
├── Filters
├── Middleware
├── Models
└── Program.cs
```

---

# 🚀 Getting Started

## 1. Clone the repository

```bash
git clone https://github.com/moh05-a/uni-system-api.git
```

## 2. Navigate to the project

```bash
cd uni-system-api
```

## 3. Configure PostgreSQL

Update your connection string inside **appsettings.json**

```json
"ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
}
```

---

## 4. Configure RabbitMQ

```json
"RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "Exchange": "unisys.events",
    "Queue": "unisys.queue",
    "RoutingKey": "student.#"
}
```

---

## 5. Apply migrations

```bash
dotnet ef database update
```

---

## 6. Run the application

```bash
dotnet run
```

---

# 📨 RabbitMQ Workflow

When a student is created:

```
POST /students
        │
        ▼
 Save Student
        │
        ▼
Publish "student.created"
        │
        ▼
RabbitMQ Exchange
        │
        ▼
Queue
        │
        ▼
Consumer
        │
        ▼
Logs / Other Services
```

This enables asynchronous communication between different parts of the system while keeping them loosely coupled.

---

# 📡 API Endpoints

## Students

| Method | Endpoint |
|---------|----------|
| GET | /api/students |
| GET | /api/students/{id} |
| POST | /api/students |
| PUT | /api/students/{id} |
| DELETE | /api/students/{id} |

---

## Tutors

| Method | Endpoint |
|---------|----------|
| GET | /api/tutors |
| POST | /api/tutors |
| PUT | /api/tutors/{id} |
| DELETE | /api/tutors/{id} |

---

## Subjects

| Method | Endpoint |
|---------|----------|
| GET | /api/subjects |
| POST | /api/subjects |
| PUT | /api/subjects/{id} |
| DELETE | /api/subjects/{id} |

---

# 📚 Design Patterns Used

- ✅ Repository Pattern
- ✅ Service Layer Pattern
- ✅ Dependency Injection
- ✅ DTO Pattern
- ✅ Publisher / Consumer Pattern
- ✅ Background Service
- ✅ Middleware Pipeline

---

# 💡 Future Improvements

- JWT Authentication
- Role-Based Authorization
- Docker Containerization
- Unit Testing
- Integration Testing
- Swagger Authentication
- Email Notifications
- Logging with Serilog
- Caching with Redis
- CI/CD Pipeline

---

# 👨‍💻 Author

**Mohammad Ameerah**

Computer Science Student • Backend Developer • ASP.NET Core Enthusiast

GitHub: https://github.com/moh05-a

---

# ⭐ Support

If you found this project helpful, consider giving it a ⭐ on GitHub.

It helps others discover the project and supports future development.
