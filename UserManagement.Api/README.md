# User Management API

- The UserManagement.API project is a robust web API designed for managing users. It includes secure mechanisms for handling user data,
API authentication via JSON Web Tokens (JWT) & IP WhiteList, and dynamic request validation through middleware.
This project also utilizes DTOs (Data Transfer Objects) for input validation and provides a streamlined API interface with Swagger documentation.**
- to create JWT token first you need to send a post request to the 'api/Users/GenerateKey' endpoint with the following body:
```json
{
  "companyName"
}
```
- The API will return a JWT token that you can use for authentication in subsequent requests.
we will also ensure that your IP address is whitelisted for security purposes.


- The User Management API includes a locking mechanism designed to ensure data consistency and prevent race conditions when multiple users attempt to create new user records simultaneously.
This lock ensures that only one user creation operation can occur at a time, avoiding potential conflicts or duplicate entries in the system
- In addition we are using Cache to store user data in memory for faster access and improved performance. we ensure to update the cache whenever we get API requests to create users.




