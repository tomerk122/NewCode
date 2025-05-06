User Management System - MongoDB Integration
Overview
This project implements MongoDB as the database backend for the Items management functionality in the User Management System. It provides a complete data access layer with repository pattern implementation and dependency injection.

MongoDB Integration
Configuration
The system connects to a MongoDB instance with the following configuration:

Connection String: mongodb://localhost:27017
Database Name: Catalog
Collection Name: items
Setup MongoDB
You can run MongoDB using Docker:

docker run -d -p 27017:27017 --name mongodb -v mongodb_data:/data/db mongo
