# Passenger Management System (PMS)

## Project Overview
The Passenger Management System (PMS) is a secure web-based application developed as an individual project to manage passenger information within an organization, such as the Irish Embassy. It supports role-based access for administrators and standard users while ensuring enterprise-level security measures aligned with **OWASP Top 10 (2025)** standards. The project demonstrates how security can be integrated throughout the Software Development Life Cycle (SDLC), from design and implementation to testing and deployment.

## Technologies Used: 
- Backend: C# (.NET 8) with Entity Framework Core  
- Database: MySQL  
- Frontend: Angular 21  
- Logging: Serilog  
- Authentication: JWT with secure key management  

## Features and Security Objectives
### Features
- Secure **Login/Logout** with role-based access (Admin/User)  
- **CRUD operations** for passenger data (Admins only)  
- **Search and List** functionality for users  
- Re-authentication for critical operations (Create, Delete, Update)  
- Centralized **Error Handling**  
- **Logging and Monitoring** of critical actions

### Security Objectives
- **Authentication & Password Security:** BCrypt password hashing, JWT tokens with short expiry  
- **Role-Based Authorization:** Admins and Users have restricted access  
- **Input Validation:** Prevent SQL injection and other malicious inputs  
- **Secure Communication:** Enforced HTTPS for client-server communication  
- **Secure Configuration:** Sensitive data stored in environment variables  
- **Logging & Monitoring:** Comprehensive audit trail using Serilog  

## Project Structure
PMS/
├── Backend/
│ ├── Controllers/ # Handles API endpoints
│ ├── Models/ # Entity models and DTOs
│ ├── Services/ # Business logic
│ ├── Data/ # Database context and migrations
│ ├── ErrorHandling.cs # Centralized error handling
│ └── Program.cs # Application entry point & JWT setup

PMS/
├── Frontend/
│ ├── src/app/components/ # Angular components (login, passenger CRUD)
│ ├── src/app/services/ # Angular services (API calls)
│ └── src/assets/ # Styles, images, and assets
├── .env # Environment variables (JWT secrets, DB)
├── README.md # Project overview and instructions
└── package.json # Frontend dependencies


## Setup and Installation

### To setup my porject follow below steps:

### Backend

- Clone the repository:

  git clone <https://github.com/Sandy07022000/Passenger_Entry_Management_Backend_CA2.git>
  cd PMS/Backend

- To restore dependencies use command:
  dotnet restore

- Configure environment variables:
  Create a .env file with the following values or save the key value in your system environment variables:
  DB_CONNECTION=<your_mysql_connection_string>
  JWT_SECRET=<your_secure_key>

- To Run the backend server use command:
  dotnet run

### Frontend

- Clone the repository:
  git clone <https://github.com/Sandy07022000/Passenger_Entry_Managment_frontend_CA2.git>
  cd PMS/Frontend
  
- Install dependencies:
  npm install

- Run the Angular application with HTTPS:
  ng serve --port 4200 --ssl true --ssl-key <path-to-key> --ssl-cert <path-to-cert>
  
- Access the application:
  Open your browser and go to https://localhost:4200.

### To download and setup the tools,applications required for running the project like visual studio, visual studio code,Angular setup,MySQl follow official documents provided by them no additionsl steps are needed.

## Usage Guidelines:

- Navigate to https://localhost:4200 in your browser.
- Login as Admin or User. Default credentials can be configured in the database.
- Admins can Create, Read, Update, Delete passenger records.
- Users can Search and List passenger records.
- Critical operations (update/delete) prompt for re-authentication.

## Security Improvements:

- BCrypt password hashing and JWT tokens with environment-based secret management.
- Role-based access control (RBAC) to enforce proper authorization.
- Input validation on frontend (Angular) and backend (C#) to prevent injection attacks.
- HTTPS enforced for all communications.
- Centralized Error Handling to prevent information leakage.
- Serilog logging for all critical actions and failed login attempts.

## Testing Process

- Functional Testing: Manual testing of all CRUD operations, login, search, and re-authentication flows. Screenshots captured for verification.
- Static Application Security Testing (SAST): Used SonarQube to scan backend code. Initial scan reveals multiple vulnerable which are fixed in various commits and final scan shows 0 security related
  vulnerability.
  
## Test Cases Examples:

- Admin login and create a passenger successfully.
- User attempting unauthorized delete operation (should fail).
- Input malicious string in search field to confirm input validation works.

## Contributions:

- Entire application developed individually from scratch.
- Used .NET 8, Angular 21, MySQL, and Serilog libraries.
- OWASP Top 10 (2025) used as reference for security implementation.
