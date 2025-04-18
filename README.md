# User Management System

## Overview
This solution consists of two main projects:

- **UserManagement** - An ASP.NET Core MVC web application
- **UserManagement.Api** - A RESTful API for user management with security features

## Web Application Features (UserManagement)

- **User Dashboard**: View all users in the system through the Users Dashboard navigation link
- **User Management**: Full CRUD operations through the `UserController`
  - Create new users
  - View user details
  - Update existing users
  - Delete users
- **Search and Filter**: 
  - Search users by username, email, or phone using the `SearchUsers` method
  - Filter users by active/inactive status

## API Features (UserManagement.Api)

- **Security**:
  - JWT authentication via the `GenerateKey` endpoint
  - IP whitelisting through the `SecurityMiddleware`
- **User Management Endpoints**:
  - Get all users
  - Get user by ID
  - Create users
  - Search users
- **Concurrency Control**: Prevents race conditions when multiple clients access the API simultaneously
- **Caching**: Improves performance through in-memory caching

## Data Storage

Both applications use a JSON file-based storage system:
- `JsonUserStorage` handles reading and writing user data to JSON files
- Data is stored in the `App_Data/Users.json` file
- Both applications implement a caching mechanism to improve performance

## User Model

The `User` model includes:
- Basic information (UserId, UserName, Password)
- Active status flag
- User group information
- Personal data in the `UserData` class (FirstName, LastName, Email, Phone, CreationDate)

## Documentation

The API includes comprehensive documentation:
- Swagger UI is configured for interactive testing
- The API README includes detailed information on endpoints, authentication, and technical implementation

## Getting Started

To run the applications:
1. Navigate to either project directory
2. Run `dotnet restore` to restore dependencies
3. Run `dotnet run` to start the application
4. For the API, browse to `/swagger` to view the API documentation

The web application is accessible at the root URL, and the API requires authentication with a JWT token obtained through the `/api/Users/GenerateKey` endpoint.
