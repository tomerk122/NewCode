# TwoServices

## Overview

TwoServices is part of a larger User Management solution that demonstrates microservices architecture using .NET 8. This project implements message-based communication between services using MassTransit and RabbitMQ.

## Project Structure

The TwoServices project is structured as follows:

- **Contracts/**: Contains shared message contracts used for service communication
- **Controllers/**: API endpoints for handling HTTP requests
- **Setting/**: Configuration settings for the application
- **Program.cs**: Application entry point and service configuration

## Features

- **Microservice Architecture**: Designed as a separate service to work alongside UserManagement and UserManagement.Api
- **Message Broker Integration**: Uses RabbitMQ for reliable message communication
- **MassTransit Framework**: Implements a robust message bus for service communication
- **RESTful API**: Exposes endpoints with Swagger documentation

## Technologies Used

- **.NET 8**: Latest .NET framework for building high-performance applications
- **MassTransit 8.4.0**: Service bus implementation for .NET
- **RabbitMQ**: Message broker for service communication
- **Swashbuckle.AspNetCore 6.6.2**: Swagger tools for API documentation

## Getting Started

### Prerequisites

- .NET 8 SDK
- RabbitMQ server running locally or accessible via network

### Installation

1. Clone the repository
2. Navigate to the TwoServices directory
3. Run `dotnet restore` to restore dependencies
4. Run `dotnet build` to build the project
5. Configure RabbitMQ connection in appsettings.json
6. Run `dotnet run` to start the service

## API Documentation

The service includes Swagger documentation available at `/swagger` when running the application.

## Integration with User Management System

This service complements the existing User Management solution by:

- Providing additional functionality through microservice architecture
- Demonstrating message-based communication between services
- Showing how to implement event-driven architecture in .NET

## Related Projects in the Solution

- **UserManagement**: ASP.NET Core MVC web application for user management
- **UserManagement.Api**: RESTful API with secure JWT authentication and IP whitelisting

## Configuration

Configuration options are available in:

- **appsettings.json**: Main configuration file
- **appsettings.Development.json**: Development-specific settings

## Development

The project uses standard .NET development practices:

- Uses dependency injection for service management
- Follows RESTful API design principles
- Implements clean architecture with separation of concerns
