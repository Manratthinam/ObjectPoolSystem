# ObjectPoolSystem

A .NET 8 Web API demonstrating the **Object Pool design pattern** using Clean Architecture.  
It pools PostgreSQL database connections and Gmail SMTP connections for efficient resource reuse.

---

## Prerequisites

| Tool | Version |
|------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0+ |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | Latest |
| PostgreSQL | 17 (via Docker) |

---

## Option 1 — Run with Docker (recommended)

### 1. Create the secrets file

Copy the example file and fill in your real credentials:

```bash
cp .env.example .env
```

Open `.env` and set your values:

```env
DB_NAME=postgres
DB_USERNAME=postgres
DB_PASSWORD=postgres
DB_PORT=5432

EMAIL_USERNAME=your-email@gmail.com
EMAIL_PASSWORD=your-gmail-app-password
```

> **Gmail App Password:** Generate one at https://myaccount.google.com/apppasswords  
> Use that instead of your Google account password.

### 2. Start the stack

```bash
docker-compose up --build
```

This starts:
- **PostgreSQL** on port `5432`
- **API** on port `8080`

### 3. Open the API

- Swagger UI → http://localhost:8080/swagger

### Stop

```bash
docker-compose down
```

To also remove the database volume:

```bash
docker-compose down -v
```

---

## Option 2 — Run locally (without Docker)

### 1. Start PostgreSQL

Make sure a PostgreSQL instance is running on `localhost:5432`.

### 2. Create the secrets file

Inside `ObjectPoolSystem.WebApi/`, create a `.env` file:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=postgres
DB_USERNAME=postgres
DB_PASSWORD=your-db-password

EMAIL_USERNAME=your-email@gmail.com
EMAIL_PASSWORD=your-gmail-app-password
```

### 3. Run the API

```bash
cd ObjectPoolSystem.WebApi
dotnet run
```

### 4. Open the API

- Swagger UI → https://localhost:7xxx/swagger  
  *(exact port is shown in the terminal output)*

---

## Architecture

![UML Diagram](./uml.png)

---

## Environment Variables Reference

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_HOST` | PostgreSQL host | `localhost` / `db` in Docker |
| `DB_PORT` | PostgreSQL port | `5432` |
| `DB_NAME` | Database name | `postgres` |
| `DB_USERNAME` | Database user | `postgres` |
| `DB_PASSWORD` | Database password | *(required)* |
| `EMAIL_USERNAME` | Gmail sender address | *(required)* |
| `EMAIL_PASSWORD` | Gmail App Password | *(required)* |
