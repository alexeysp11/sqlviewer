# sqlviewer

[English](README.md) | [Русский](README.ru.md)

`sqlviewer` — это многоязычное приложение C#, предоставляющее удобный интерфейс для взаимодействия с различными СУБД и выполнения различных операций с базами данных.

## Общее описание

Данное приложение позволяет выполнять следующие операции:

- писать и исполнять SQL-запросы:

![ui_query](docs/img/ui_query.png)

- перенос данных из одной БД в другую:

![ui_etl_1](docs/img/ui_etl_1.png)

![ui_etl_2](docs/img/ui_etl_2.png)

## Запуск проекта в docker

1. Создайте сертификат для поддержки протокола HTTPS и установите пароль:
```bash
# For Windows (PowerShell)
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust

# For Linux/macOS
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust
```
2. Передите в корневую папку данного проекта.
3. Создайте файл `.env` и добавьте в него информацию о вашем пароле: например, `CERT_PASSWORD=YourSecurePassword123`. Также добавьте путь, где хранится HTTPS сертификат в качестве переменной `HOST_CERT_PATH`.
4. Для сборки и запуска всего стека приложений: `docker compose up --build`.

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
