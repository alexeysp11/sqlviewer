# sqlviewer

[English](README.md) | [Русский](README.ru.md)

`sqlviewer` — это многоязычное приложение C#, предоставляющее удобный интерфейс для взаимодействия с различными СУБД и выполнения различных операций с базами данных.

## Общее описание

Данное приложение позволяет выполнять следующие операции:

- писать и исполнять SQL-запросы:

![Example (UI, query)](docs/img/ui_query.png)

- Просматривать информацию обо всех таблицах внутри БД (SQL-определение таблицы, колонки/поля, внешние ключи, триггеры и все данные внутри конкретной таблицы):

![Example (UI, tables)](docs/img/ui_tables.png)

- перенос данных из одной БД в другую:

![Example (UI, connections)](docs/img/ui_connections.png)

## JSON

`/api/sql/create-table`:
```json
{
    "databaseType": 3,
    "tableName": "NewCreatedTable",
    "columns": [
        {
            "databaseType": 3,
            "columnName": "Id",
            "columnType": 12
        },
        {
            "databaseType": 3,
            "columnName": "Column1",
            "columnType": 16
        },
        {
            "databaseType": 3,
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
