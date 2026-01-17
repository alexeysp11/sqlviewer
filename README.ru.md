# sqlviewer

[English](README.md) | [Русский](README.ru.md)

Распределенная система управления гетерогенными источниками данных.

## Общее описание

- Микросервисная архитектура.
- Docker контейнеры для сервисов и баз данных.
- Централизованное хранилище метаданных (Metadata Storage) и абстракция подключений.
- Система позволяет пользователям взаимодействовать с различными БД (SQLite, PostgreSQL, MS SQL) через унифицированный API, скрывая технические детали реализации подключений.
- Автоматические миграции и заполнение данными при старте сервиса (для ускорения разработки).

Так как в качестве основы данной системы используется WPF и Web API, то стоит подчеркнуть, что микросервисы выбраны для масштабируемости: например, сервис выполнения запросов можно масштабировать отдельно от сервиса управления метаданными, если нагрузка на БД возрастет.

## Функционал

### Подключение к базам данных

Приложение поддерживает два способа идентификации целевой базы данных для выполнения запросов:
- Использование явного connection string
- Использование источников данных (data sources)

#### Использование явного connection string

Классический способ, при котором в поле ввода передается полная строка подключения (например, для PostgreSQL или SQLite). Такой способ позволяет быстро подключаться к любой доступной базе без предварительной настройки.

#### Использование источников данных (data sources)

Механизм, позволяющий обращаться к базам через именованные профили, настроенные на сервере. Вместо Connection String используется специальный тег:
- `[DataSource Id="1"]`: поиск по уникальному идентификатору.
- `[DataSource Name="pg-metadata-db"]`: поиск по понятному имени.
- `[DataSource Id="1" Name="pg-metadata-db"]`: поиск по обоим параметрам (строгое соответствие).

Преимущества этого подхода:
- **Безопасность**: Пользователь и клиентское приложение не видят пароли и адреса серверов. Все чувствительные данные хранятся на стороне ASP.NET Core сервера.
- **Удобство**: Вместо запоминания сложных строк подключения используются читаемые алиасы (например, "Warehouse 2026", "Analytics DB").
- **Гибкость**: Если адрес базы данных изменится, администратору достаточно обновить его на сервере — клиентам не нужно менять настройки в своих запросах.

### Писать и выполнять SQL-запросы

![ui_query](docs/img/ui_query.png)

### Перенос данных из одной БД в другую

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
3. Создайте файл `.env` и добавьте в него данные для конфигурации webapi внутри docker контейнера.
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
