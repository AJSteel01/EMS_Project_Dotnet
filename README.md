# Employee Management System (EMS)

A full-stack Employee Management System built with a secure, role-based architecture for managing employees efficiently.

---

## Tech Stack

- **Backend:** ASP.NET Core Minimal API (.NET 8/9/10), Entity Framework Core, SQL Server, Identity (cookie authentication)
- **Frontend:** Blazor Server
- **Architecture:** Role-based access (Admin and Employee), secure cookies, pagination, DTO mapping, and validation

---

## Features

The system supports two roles: **Admin** and **Employee**.

- Both Admin and Employee can log in and view their own profile.
- Admin can edit all details of any employee, while Employees can edit only their own phone number and address.
- Admin can view all employees, add new employees, edit any employee’s details, delete employees, and assign roles (Admin/Employee).
- Employees cannot view the full employee list, add, edit, or delete other employees.

---

## Project Structure

The solution consists of two main projects:

- **EMSBackend** – ASP.NET Core Minimal API  
  Contains:
  - `Data/`
  - `Endpoints/`
  - `Dtos/`
  - `Entities/`
  - `Mapping/`
  - `Program.cs`

- **EMSFrontend** – Blazor Server application  
  Contains:
  - `Components/`
  - `Clients/`
  - `Models/`
  - `Services/`
  - `Program.cs`

---

## Backend Details

The backend uses:

- .NET Minimal APIs for lightweight endpoints
- Entity Framework Core for data access
- SQL Server as the database
- ASP.NET Core Identity with cookie-based authentication
- Authorization policies such as `AdminOnly` and `EmployeeOnly`
- DTOs and mapping for clean separation between entities and API contracts

---

## Frontend Details

The frontend uses:

- Blazor Server with Bootstrap for UI
- Cookie-based authentication integrated via an `HttpClient` handler
- `EditForm` components with validation for forms
- Protected routes and role-based UI to show or hide pages/actions depending on the logged-in user

---

## Database Configuration

Update your connection strings in `EMSBackend/appsettings.json`:

```json

"ConnectionStrings": {
"EMS": "Server=.;Database=EMSDB;Trusted_Connection=True;TrustServerCertificate=True",
"AuthDB": "Server=.;Database=EMSAuthDB;Trusted_Connection=True;TrustServerCertificate=True"
}

```


Ensure SQL Server is running and adjust the server name and options as needed for your environment.

---

## Running the Application

1. **Start the Backend API**

```
cd EMSBackend
dotnet run
```

By default, the API runs at `http://localhost:5222`.

2. **Start the Frontend (Blazor Server)**

```
cd EMSFrontend
dotnet watch run
```


By default, the frontend runs at `http://localhost:5083`.

---

## Default Login

- **Admin user**
- Email: `admin@ems.com`
- Password: `Admin@123`

Change the default credentials before deploying to any production environment.

---

## Main API Endpoints

Authentication:

- `POST /auth/login` – Log in and create an authenticated session
- `GET /auth/me` – Get the currently logged-in user

Employees:

- `GET /employees` – Get a paginated list of employees (Admin only)
- `GET /employees/{id}` – Get a specific employee (Admin or the employee themself)
- `POST /employees` – Add a new employee (Admin only)
- `PUT /employees/{id}` – Update an employee (Admin only)
- `PUT /employees/self` – Employee updates their own profile
- `DELETE /employees/{id}` – Delete an employee (Admin only)
