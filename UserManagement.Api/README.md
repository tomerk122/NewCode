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
- ** Credtentials**: Secure storage of authentication credentials in a JSON file (manager who can access the API)

## Getting Started

### Installation

1. Clone the repository
2. Navigate to the UserManagement.Api directory
3. Run `dotnet restore` to restore dependencies
5. Run `dotnet run` to start the API

## Authentication

### Generating a JWT Token

To create a JWT token, send a POST request to the `api/Users/GetToken` endpoint with the following body:

```
curl -X 'POST' \
  'http://127.0.0.1:5110/api/Users/GetToken' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "userName": "Amit",
  "password": "qwe12www",
  "company": "RICHKID"
}'
```

The API will return a JWT token that you can use for authentication in subsequent requests. 
Your IP address will be checked against a whitelist to ensure that only authorized users can generate tokens.
(can be cancelled by the manager)
**you can get a token ONLY if you are in the Credintials.json file**

### Using the JWT Token

Include the token in the Authorization header of your requests:

```
Authorization: Bearer {your_jwt_token}
```

## API Endpoints

### User Management

- `GET api/GetAllUsers` - Get all users
- `GET api/Users/{Userid}` - Get user by ID
- `POST api/Users/AddUSer` - Create a new user
- `POST api/Users/SearchUser` - Search a user by his First Name Or last
- `Post api/Users/GetToken` - Get token for authentication, User need to login to get a token


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
│   ├── Users.json.bak             # Backup file for user data
│   └── Credentials.json           # Stores authentication credentials
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
│   │   ├── CredentialsDto.cs      # DTO for authentication credentials
│   │   ├── SearchUserDto.cs       # DTO for user search
│   │   └── Validators/            # Input validation
│   │       └── DtoValidator.cs    # Validates DTO objects
│   └── User.cs                    # User entity model
│
├── Repositories/                  # Data access layer
│   ├── JsonUserStorage.cs         # JSON file storage for users
│   ├── JsonCredentialsManager.cs  # Manages authentication credentials
│   └── UserRepository.cs          # User data operations
│
├── Program.cs                     # Application entry point and setup
├── appsettings.json               # Application configuration
```

### Key Components Explanation

- **App_Data**: Contains the JSON file used for storing user data, 
 Credentials.json for storing authentication credentials, and a backup file for data safety.

- **Controllers**: Contains the UsersController which handles all HTTP requests for user management and authentication.

- **Middleware**: 
  - IP validation components ensure requests come from authorized sources
  - JWT generation and validation secures API access by the credentials.json file. only users in the file can get a token
  - SecurityMiddleware combines these security features into the request pipeline

- **Models**: 
  - DTOs handle input validation for various operations
  - The User class represents the core data entity

- **Repositories**: 
  - JsonUserStorage provides the file-based persistence layer
  - UserRepository implements the business logic for data access with concurrency control
  - JsonCredentialsManager manages the authentication credentials stored in the JSON file

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
  'https://localhost:7011/api/Users/GetToken' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "userName": "Amit",
  "password": "qwe12www",
  "company": "RICHKID"
}'
 
```
and etc..

**Remember to replace `<port>` with your actual API port and `your_jwt_token` with the token received from the GenerateToken endpoint.**
![image](https://github.com/user-attachments/assets/0c710dbf-d4dd-4a10-8cc6-72cd824d3ce1)








