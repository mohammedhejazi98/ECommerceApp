# ECommerceApp

ECommerceApp is a microservices-based project consisting of three main services: OrderService, InventoryService, and NotificationService. These services work together to manage orders, inventory, and notifications for an e-commerce platform.

## Projects

1. **OrderService**: Manages customer orders.
2. **InventoryService**: Manages inventory levels and availability.
3. **NotificationService**: Handles notifications related to orders and inventory.

## Requirements

- .NET 8.0 SDK
- Docker
- Docker Compose

## Building and Running the Solution

### Using .NET CLI

To build and run the solution locally:

```sh
dotnet build
dotnet run --project OrderService/OrderService.csproj
dotnet run --project InventoryService/InventoryService.csproj
dotnet run --project NotificationService/NotificationService.csproj
```

### Using Docker and Docker Compose

Ensure Docker and Docker Compose are installed on your machine.

#### Docker Setup

Each service has its own Dockerfile, so you can build and run them individually using Docker.

1. **OrderService**:
    ```sh
    docker build -t orderservice -f OrderService/Dockerfile .
    docker run -d -p 5001:80 orderservice
    ```

2. **InventoryService**:
    ```sh
    docker build -t inventoryservice -f InventoryService/Dockerfile .
    docker run -d -p 5002:80 inventoryservice
    ```

3. **NotificationService**:
    ```sh
    docker build -t notificationservice -f NotificationService/Dockerfile .
    docker run -d -p 5003:80 notificationservice
    ```

Alternatively, you can manage all the services using Docker Compose.

#### Docker Compose Setup

1. Make sure you have a `docker-compose.yml` file configured for your services.
2. To build and start all services defined in the `docker-compose.yml`:

    ```sh
    docker-compose up --build
    ```

3. To stop the services:

    ```sh
    docker-compose down
    ```

## Configuration

- Ensure environment variables and related configurations are appropriately set for each service.
- Refer to individual service README files or documentation for specific configuration details.

## Logging

All services are configured to use `Microsoft.Extensions.Logging` for structured logging. Logs will be output to the console and can be viewed in the Docker logs or terminal output.

## Contributing

To contribute to this project, please fork the repository and submit a pull request. Ensure all new code is adequately tested and documented.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
