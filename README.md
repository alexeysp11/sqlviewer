# sqlviewer

[English](README.md) | [Русский](README.ru.md)

A distributed system for managing heterogeneous data sources.

## General description

- Microservice architecture.
- Centralized metadata storage and connection abstraction.
- The system allows users to interact with various databases (SQLite, PostgreSQL, MS SQL) through a unified API, hiding the technical details of connection implementation.
- Automatic migrations upon service launch (to speed up development).

Since this system is based on WPF and Web API, it's worth noting that microservices were chosen for scalability: for example, the query execution service can be scaled separately from the metadata management service if the load on the database increases.

## Features

Using this app, you can do the following things:

- Write and execute SQL queries:

![ui_query](docs/img/ui_query.png)

- Transfer data from one database to another:

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
DB_HOST=host.docker.internal
DB_PASSWORD=StrongPassword1234

HOST_CERT_PATH=path
CERT_PASSWORD=AnotherStrongPassword1234
```
4. To build and run the entire application stack: `docker compose up --build`.

## JSON

`/api/sql/create-table`:
```json
{
    "databaseType": 3,
    "tableName": "NewCreatedTable",
    "columns": [
        {
            "columnName": "Id",
            "columnType": 12
        },
        {
            "columnName": "Column1",
            "columnType": 16
        },
        {
            "columnName": "Column2",
            "columnType": 16
        }
    ],
    "connectionString": "Server=localhost;Username=postgres;Database=postgres;Port=5432;Password=postgres"
}
```

`/api/sql/drop-table`:
```json
{
    "databaseType": 3,
    "tableName": "NewCreatedTable",
    "connectionString": "Server=localhost;Username=postgres;Database=postgres;Port=5432;Password=postgres"
}
```
