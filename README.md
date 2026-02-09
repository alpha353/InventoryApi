# Inventory Management API

A robust, containerized Web API for managing product inventory and stock transactions. Built with ASP.NET Core 8 and PostgreSQL.

## 🚀 Features

- **Product Management**: Create, read, and search products.
- **Stock Tracking**: Real-time stock level calculation based on transaction history.
- **Transactional Consistency**: All stock changes (sales, restocks, adjustments) are recorded as immutable transactions.
- **Dockerized**: Fully containerized environment with automated database seeding.
- **Resilient**: Implements retry logic for database connectivity and robust exception handling.
- **documented**: Integrated Swagger UI for interactive API exploration.

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0 (Web API)
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core
- **Validation**: FluentValidation
- **Containerization**: Docker & Docker Compose

## 🏁 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.

### Installation & Run

1.  **Clone the repository** (or navigate to the project root).
2.  **Start the Services**:
    ```powershell
    docker-compose up --build
    ```
    *This command will build the API image, pull the PostgreSQL image, and start both services in a private network.*

3.  **Access the API**:
    - **Swagger UI**: [http://localhost:8080/index.html](http://localhost:8080/index.html)
    - **API Root**: [http://localhost:8080](http://localhost:8080)

## 📡 API Reference

### Products

- **GET /api/Inventory/products**
  - Retrieves a paginated list of products.
  - Supports search: `?search=laptop`

### Inventory

- **GET /api/Inventory/{productId}/level**
  - Returns current stock quantity for a product.
- **POST /api/Inventory/adjust**
  - Records a stock change (Sale, Restock, Adjustment).
  - *Example Body:*
    ```json
    {
      "productId": 1,
      "quantityChange": -1,
      "type": 1
    }
    ```
    *(Type: 0=Adjustment, 1=Sale, 2=Restock, 3=Return)*

## 🏗️ Architecture

- **Controllers**: Lean entry points handling HTTP requests/responses.
- **Repositories**: Encapsulate data access logic (`ProductRepository`, `InventoryRepository`).
- **Services/Logic**: Domain logic ensures stock cannot be negative on sales.
- **Data Layer**: EF Core `DbContext` managing `Product` and `StockTransaction` entities.
- **Middleware**: Global exception handler for standardized error responses.

## 🧪 Verification

To verify the deployment is working correctly:

1.  Open **[http://localhost:8080/index.html](http://localhost:8080/index.html)**.
2.  Execute `GET /api/Inventory/products`.
3.  You should receive a JSON response with 5 seeded products (Laptop, Mouse, etc.).
