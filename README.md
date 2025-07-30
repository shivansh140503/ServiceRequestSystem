# Service Request Management System (Backend)

A backend API for a Service Request Management System built with **ASP.NET Core Web API** and **SQL Server**.  
This API supports CRUD operations on service requests, user authentication with JWT, and follows clean architecture practices.

## 📁 Repository Structure

ServiceRequestSystem/
├── Backend/
│ └── ServiceRequestAPI/ # .NET Core Web API project
└── Frontend/ # Angular app (Work in Progress)
└── service-request-ui/


## 🛠 Tech Stack

- **Framework:** ASP.NET Core 8
- **Database:** SQL Server (LocalDB)
- **ORM:** Entity Framework Core
- **API Docs:** Swagger/OpenAPI

## ✅ Features

- CRUD operations for service requests
- Repository pattern with separate files per operation
- DTOs for clean data transfer
- Global exception handling
- Swagger integration for API testing

## 🔧 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or LocalDB
- Visual Studio / VS Code

### Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/shivansh140503/ServiceRequestSystem.git
   cd ServiceRequestSystem/Backend/ServiceRequestAPI
   
2. Configure the database connection in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ServiceRequestAPI;Trusted_Connection=True;"
}

3. Run EF Core migrations:

dotnet ef database update

4.Run the application:

dotnet run

5.Visit Swagger UI at:

http://localhost:5000/swagger

🧪 API Testing
You can use tools like Postman or Swagger UI to test the API endpoints for:

/api/auth/register – Register a new user

/api/auth/login – Login and receive JWT

/api/servicerequests – Manage service requests (CRUD)

🚧 Frontend
The Angular frontend is currently under development and located in:

Frontend/service-request-ui/

📄 License
This project is licensed under the MIT License.

Created by Shivansh Parganiha


---

Let me know when the frontend is ready and I’ll help you update the READ
