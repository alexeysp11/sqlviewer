# sqlviewer

[English](README.md) | [Русский](README.ru.md)

A distributed system for managing heterogeneous data sources.

## 📖 General description

- **Microservice architecture**: Scalable and decoupled components.
- **Docker-ready**: Fully containerized services and databases.
- **Abstraction layer**: Interact with various databases (SQLite, PostgreSQL, MS SQL) through a unified API.
- **Auto-migrations**: Database seeding and migrations on startup for rapid development.

## 🏗️ Architecture

- **WPF Client**: Sends an HTTPS request to **API Gateway**.
- **API Gateway**: Performs routing and calls the required microservice via gRPC.
- **Security Service**: Authorizes the user in the application.
- **Metadata Service**: Provides connection parameters to the target database.
- **Query Execution Service**: Executes an SQL query in the selected database (internal or external).

## ✨ Features

### 🔌 Connecting to Databases

The application supports two methods for identifying the target database:
- **Explicit Connection String**: Quick connection without prior configuration.
- **Data Sources**: Named profiles (e.g., `[DataSource Name="Analytics"]`) for enhanced security and convenience.

### 📝 SQL Query Execution

![ui_query](docs/img/ui_query.png)

### 🔄 Data Transfer (ETL)

Transfer data seamlessly between different database engines.

## 🗄️ System Databases & Sandbox

The system automatically initializes several PostgreSQL databases upon startup. You don't need to create them manually.

### 🛡️ Internal Databases
These are used by the microservices to store system metadata and security logic:
- **`sqlviewer-security`**: Stores users, roles, and permissions for the **Security** service (*connection string*: `[DataSource Name="pg-security-db"]`).
- **`sqlviewer-metadata`**: Stores Data Source configurations and connection abstractions (*connection string*: `[DataSource Name="pg-metadata-db"]`).
- **`sqlviewer-query`**: Stores query history and execution logs (*connection string*: `[DataSource Name="pg-query-db"]`).
- **`sqlviewer-etl`**: Stores ETL commands and messages (*connection string*: `[DataSource Name="pg-etl-db"]`).

### 🏖️ Sandbox Database (Quick Start)
For testing purposes, a **`sqlviewer-sandbox`** is provided.
- It comes pre-filled with sample tables (e.g., `Employees`, `Orders`).
- You can use it immediately after launch to test SQL queries or ETL processes.
- **Default DataSource Name:** `[DataSource Name="pg-sandbox-db"]`

> **Note:** All migrations and seed data are applied automatically via Entity Framework Core on service startup.

## 🐳 Running a project in Docker

### 1. 🔒 Generate HTTPS Certificate

```bash
# For Windows (PowerShell)
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust

# For Linux/macOS
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p YourSecurePassword123
dotnet dev-certs https --trust
```

### 2. 📝 Environment Setup

Create a .env file in the root directory:
```.env
# Database server
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
```

### 3. 🚀 Launch

`docker compose up --build`.

## 📊 Observability & Monitoring

The monitoring stack is deployed automatically alongside the core services.

### How to verify it's working?

Spin up the infrastructure: `docker-compose up -d`

#### 1. Prometheus (Metrics Collection)
Used for gathering and storing performance metrics from all your services.
- **URL:** [http://localhost:9090](http://localhost:9090)
- **Check:** Navigate to `Status` -> `Targets`.
- **Expected Result:** All services (`api-gateway`, `security`, `metadata`, etc.) should have an **UP** status. This confirms Prometheus is successfully scraping data from the `:8080/metrics` endpoints.

> **🔐 Security Note (Production-ready):**  
> For this pet project, metrics ports (e.g., `:5103`, `:8080`) are forwarded externally in `docker-compose.yml` to facilitate manual inspection of the `/metrics` endpoints from a browser. In a real production environment, these ports should be closed to external access and accessible only within the Docker virtual network for Prometheus itself.

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

## 🛠️ Known Issues / Future Improvements

This section outlines technical debt and architectural considerations identified during the current development phase. These points are planned for future iterations to enhance system resilience and scalability.

### 1. Thundering Herd Problem in Polling
Currently, multiple desktop clients might synchronize their status polling requests, potentially leading to significant spikes in load on the API Gateway and ETL Service (the "Thundering Herd" effect).
* **Proposed Solution:** Implement **Jitter** (adding a small random delay to each request) to distribute the load more evenly over time.
* **Error Handling:** Implement **Exponential Backoff** to increase the delay between retries if the server returns 5xx errors or rate-limit responses (429).

### 2. Client-Side Resource Optimization
The current background worker for active operations maintains a constant polling interval regardless of the application's state.
* **Proposed Solution:** Implement an adaptive polling strategy. The interval should increase (e.g., from 2s to 30s) if the application window is **minimized**, reducing unnecessary network traffic and CPU usage on both the client and server sides.

### 3. Distributed Tracing Continuity (Jaeger)
Due to batch processing in the ETL Worker and Data Transfer Worker, the `CorrelationId` trace context may occasionally break when messages are grouped.
* **Proposed Solution:** Manually propagate the `SpanContext` by extracting trace headers from individual messages within a batch and creating child spans for each processing task.
