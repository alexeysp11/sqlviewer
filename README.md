# sqlviewer

[English](README.md) | [Русский](README.ru.md)

`sqlviewer` is a desktop application that provides a user-friendly interface for interacting with different RDBMS and performing various database operations.

## Overall description

Using this app, you can do the following things:

- write and execute SQL queries:

![ui_query](docs/img/ui_query.png)

- transfer data from one database to another:

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
3. Create a `.env` file and add your password information to it: for example, `CERT_PASSWORD=YourSecurePassword123`. Also add the path where the HTTPS certificate is stored as the `HOST_CERT_PATH` variable.
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
