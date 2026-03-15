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
DB_PASSWORD=postgres

# Encryption
ENCRYPTION_KEY=YourSuperSecretKey123

# HTTP certificate
# For Windows
HOST_CERT_PATH=${USERPROFILE}\.aspnet\https
# For Linux/macOS it could be: HOST_CERT_PATH=~/.aspnet/https

CERT_PASSWORD=1234

# JWT
JWT_KEY=TMbQYRKQfN5kG+LfcDocF6ezcs8HL/EK4pBZ5V9UZwo=
JWT_LIFETIME_MINUTES=90
JWT_ISSUER=sqlviewer/security
JWT_AUDIENCE=sqlviewer/api-gateway
JWT_ISSUER_KEY=iZSSPdC53bxc301eVH/bN6eTjzmXkMIhvMbxfcn0q8k=

# Microservices
GRPC_SECURITY_HOST=host.docker.internal
GRPC_SECURITY_PORT=5275
GRPC_METADATA_HOST=host.docker.internal
GRPC_METADATA_PORT=5101
GRPC_QUERYEXECUTION_HOST=host.docker.internal
GRPC_QUERYEXECUTION_PORT=5250
```
4. To build and run the entire application stack: `docker compose up --build`.

## 📊 Observability & Monitoring

The monitoring stack is deployed automatically alongside the core services.

### How to verify it's working?

Spin up the infrastructure: `docker-compose up -d`

#### 1. Prometheus (Metrics Collection)
Used for gathering and storing performance metrics from all your services.
- **URL:** [http://localhost:9090](http://localhost:9090)
- **Check:** Navigate to `Status` -> `Targets`.
- **Expected Result:** All services (`api-gateway`, `security`, `metadata`, etc.) should have an **UP** status. This confirms Prometheus is successfully scraping data from the `:8080/metrics` endpoints.

#### 2. Jaeger (Distributed Tracing)
Allows you to visualize the request lifecycle across microservices (from API Gateway to DB).
- **URL:** [http://localhost:16686](http://localhost:16686)
- **Check:** 
    1. Send any request to the API (via WPF client or Swagger).
    2. In Jaeger, select `api-gateway` in the **Service** field.
    3. Click **Find Traces**. You will see the timeline (Spans) of your request and its gRPC calls.

#### 3. Grafana (Visualization)
Rich dashboards for real-time system health monitoring.
- **URL:** [http://localhost:3000](http://localhost:3000)
- **Credentials:** `admin` / `admin`
- **Initial Setup:**
    1. Go to `Connections` -> `Data Sources` -> `Add data source`.
    2. Select **Prometheus**.
    3. In the **URL** field, enter `http://prometheus:9090` and click **Save & Test**.

### Importing the Standard .NET Dashboard

For deep analysis of ASP.NET Core services, it is highly recommended to use the pre-built dashboard.

1. In Grafana, click **Dashboards** -> **New** -> **Import**.
2. In the `"Import via grafana.com"` field, enter ID: `19924` and click **Load**.
3. In the dropdown, select the **Prometheus** DataSource you created.
4. Click **Import**.

**Dashboard Highlights:**
*   **Requests per second:** Incoming traffic intensity.
*   **Request Duration:** API response latency.
*   **HTTP Error Rate:** Percentage of `4xx`/`5xx` errors.
*   **Resource Usage:** CPU and RAM consumption per microservice.
