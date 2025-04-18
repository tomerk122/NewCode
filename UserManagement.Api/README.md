# User Management API

## Overview

The UserManagement.API project is a web API designed for managing users. It includes secure mechanisms for handling user data, API authentication via JSON Web Tokens (JWT) & IP WhiteList, and dynamic request validation through middleware. This project also utilizes DTOs (Data Transfer Objects) for input validation and provides a streamlined API interface with Swagger documentation.

## Features

- **JWT Authentication**: Secure API access using JSON Web Tokens
- **IP Whitelisting**: Additional security through IP address validation
- **Concurrency Control**: Locking mechanism to prevent race conditions
- **Caching**: In-memory caching for improved performance
- **Input Validation**: Using DTOs for request validation
- **API Documentation**: Comprehensive Swagger documentation
- **synchronization mechanism**: Can handle multiple users trying to use the API for our system

## Getting Started

### Installation

1. Clone the repository
2. Navigate to the UserManagement.Api directory
3. Run `dotnet restore` to restore dependencies
4. Update the connection string in `appsettings.json`
5. Run `dotnet run` to start the API

## Authentication

### Generating a JWT Token

To create a JWT token, send a POST request to the `api/Users/GenerateKey` endpoint with the following body:

```
curl -X 'POST' \
  'http://127.0.0.1:5110/api/Users/GenerateKey' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '<Company-Name>'
```

The API will return a JWT token that you can use for authentication in subsequent requests. Your IP address will also be automatically whitelisted for security purposes.

### Using the JWT Token

Include the token in the Authorization header of your requests:

```
Authorization: Bearer {your_jwt_token}
```

## API Endpoints

### User Management

- `GET api/Users` - Get all users
- `GET api/Users/{id}` - Get user by ID
- `POST api/Users` - Create a new user
- `PUT api/Users/{id}` - Update an existing user
- `DELETE api/Users/{id}` - Delete a user

### Authentication

- `POST api/Users/GenerateKey` - Generate a JWT token for API authentication

## Technical Implementation Details

### Concurrency Control

The User Management API includes a locking mechanism designed to ensure data consistency and prevent race conditions when multiple users attempt to create new user records simultaneously. This lock ensures that only one user creation operation can occur at a time, avoiding potential conflicts or duplicate entries in the system.

### Caching

We use in-memory caching to store user data for faster access and improved performance. The cache is automatically updated whenever API requests to create, update, or delete users are processed, ensuring data consistency between the cache and the underlying database.

### Request Validation

All incoming requests are validated through middleware and DTOs before processing, ensuring data integrity and preventing invalid operations.

## Project Structure

```
/UserManagement.Api/
├── App_Data/                      # Data storage location
│   ├── Users.json                 # JSON file storing user data
│
├── Controllers/                   # API endpoints and request handling
│   └── UsersController.cs         # Handles all user-related requests
│
├── Middleware/                    # Custom HTTP pipeline components
│   ├── IpValidator.cs             # Validates IP addresses against whitelist
│   ├── JWT/                       # JWT authentication components
│   │   └── JwtTokenGenerator.cs   # Generates JWT tokens
│   ├── JwtValidator.cs            # Validates incoming JWT tokens
│   └── SecurityMiddleware.cs      # Handles auth and IP security
│
├── Models/                        # Data models and DTOs
│   ├── DTOS/                      # Data Transfer Objects
│   │   ├── CreateUserDto.cs       # DTO for user creation
│   │   ├── SearchUserDto.cs       # DTO for user search
│   │   └── Validators/            # Input validation
│   │       └── DtoValidator.cs    # Validates DTO objects
│   └── User.cs                    # User entity model
│
├── Repositories/                  # Data access layer
│   ├── JsonUserStorage.cs         # JSON file storage implementation
│   └── UserRepository.cs          # User data operations
│
├── Program.cs                     # Application entry point and setup

```

### Key Components Explanation

- **App_Data**: Contains the JSON file used for storing user data, with a backup mechanism to prevent data loss.

- **Controllers**: Contains the UsersController which handles all HTTP requests for user management and authentication.

- **Middleware**: 
  - IP validation components ensure requests come from authorized sources
  - JWT generation and validation secures API access, and Company
  - SecurityMiddleware combines these security features into the request pipeline

- **Models**: 
  - DTOs handle input validation for various operations
  - The User class represents the core data entity

- **Repositories**: 
  - JsonUserStorage provides the file-based persistence layer
  - UserRepository implements the business logic for data access with concurrency control

## Swagger Documentation

To access the Swagger UI documentation for this API:

1. Start the API by running `dotnet run`
2. Open a web browser and navigate to:
  - `https://localhost:<port>/swagger` (if using HTTPS)
  - `http://localhost:<port>/swagger` (if using HTTP)
  
The Swagger UI provides interactive documentation where you can explore and test all available endpoints.

## Manual API Requests

You can also interact with the API manually using tools like Postman, curl, or any HTTP client:

### Authentication Request
```bash
curl -X 'POST' \
  'http://127.0.0.1:5110/api/Users/GenerateKey' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '<Company-Name>'
 
```

### Get All Users
```bash
curl -X 'GET' \
  'http://127.0.0.1:5110/api/Users/GetAllUsers' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJDb21wYW55IjoiQXBwbGUiLCJuYmYiOjE3NDQ5NzQ3NjIsImV4cCI6MTc0NDk3ODM2MiwiaWF0IjoxNzQ0OTc0NzYyLCJpc3MiOiJVc2VyTWFuYWdlbWVudC5BcGkiLCJhdWQiOiJVc2VyTWFuYWdlbWVudC5BcGkifQ.pbraBtnOMZ60pCuVPeKKj1d5BzPO38lVzUxZ4ZByw7Q'
```

### Create New User
```bash
curl -X POST "http://localhost:<port>/api/Users" \
    -H "Content-Type: application/json" \
    -H "Authorization: Bearer your_jwt_token" \
    -d '{
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com",
        "phone": "123-456-7890",
        "role": "User"
      }'
```

Remember to replace `<port>` with your actual API port and `your_jwt_token` with the token received from the GenerateKey endpoint.



