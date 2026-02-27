# sqlviewer

[English](README.md) | [Русский](README.ru.md)

A distributed system for managing heterogeneous data sources.

## General description

- Microservice architecture.
- Docker containers for services and databases.
- Centralized metadata storage and connection abstraction.
- The system allows users to interact with various databases (SQLite, PostgreSQL, MS SQL) through a unified API, hiding the technical details of connection implementation.
- Automatic migrations and data seeding upon service launch (to speed up development).

Since this system is based on WPF and Web API, it's worth noting that microservices were chosen for scalability: for example, the query execution service can be scaled separately from the metadata management service if the load on the database increases.

## Features

### Connecting to databases

The application supports two methods for identifying the target database for query execution:
- Using an explicit connection string
- Using data sources

#### Using an explicit connection string

The classic method, in which the full connection string is passed in the input field (for example, for PostgreSQL or SQLite). This method allows you to quickly connect to any available database without prior configuration.

#### Using data sources

A mechanism for accessing databases through named profiles configured on the server. A special tag is used instead of the donnection string:
- `[DataSource Id="1"]`: search by unique identifier.
- `[DataSource Name="pg-metadata-db"]`: search by user friendly name.
- `[DataSource Id="1" Name="pg-metadata-db"]`: search by both parameters (strict matching).

The advantages of this approach:
- **Security**: The user and client application don't see passwords or server addresses. All sensitive data is stored on the ASP.NET Core server side.
- **Convenience**: Instead of remembering complex connection strings, human-readable aliases are used (e.g., "Warehouse 2026," "Analytics DB").
- **Flexibility**: If the database address changes, the administrator simply updates it on the server, and clients don't need to change settings in their requests.

### Write and execute SQL queries:

![ui_query](docs/img/ui_query.png)

### Transfer data from one database to another:

![ui_etl_1](docs/img/ui_etl_1.png)

![ui_etl_2](docs/img/ui_etl_2.png)

## Running a project in Docker

1. Create a certificate to support the HTTPS protocol and set a password:
```bash
# For Windows (PowerShell)
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust

# For Linux/macOS
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust
```
2. Navigate to the root folder of this project.
3. Create a `.env` file and add the configuration data for webapi inside the docker container.
```.env
# Database server
DB_HOST=host.docker.internal
DB_PASSWORD=DatabasePassword1234

# Encryption
ENCRYPTION_KEY=YourSuperSecretKey123

# HTTP certificate
HOST_CERT_PATH=path
CERT_PASSWORD=YourSecurePassword123
```
4. To build and run the entire application stack: `docker compose up --build`.
