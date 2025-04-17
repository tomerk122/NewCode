# User Management System

A simple ASP.NET Core MVC application for managing users with support for creating, reading, updating, and deleting user records. The data is stored in a JSON file and cached for performance.


## Project Structure

- **Models**: Contains the `User` and `UserData` classes defining data structure
- **Controllers**: 
  - `HomeController`: Handles the homepage
  - `UserController`: Manages all user-related operations
- **Repositories**:
  - `UserRepository`: Business logic for user management
  - `JsonUserStorage`: Handles JSON file operations
	- `UserCache`: Caches user data for performance
- **Views**: Razor views for presenting the user interface

## Features

- **View all users**: Displays all users in the system.
- **Add new user**: Add a new user with a unique username.
- **Update existing user**: Modify user details.
- **Delete user**: Remove a user from the system.
- **Search users**: Search users by username, email, or phone.
- **Filter by status**: Filter users by their active/inactive status.

