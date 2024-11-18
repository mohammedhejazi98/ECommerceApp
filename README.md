# ECommerceApp

ECommerceApp is a microservices-based project consisting of three main services: OrderService, InventoryService, and NotificationService. These services work together to manage orders, inventory, and notifications for an e-commerce platform.

## Projects

1. **OrderService**: Manages customer orders.
2. **InventoryService**: Manages inventory levels and availability.
3. **NotificationService**: Handles notifications related to orders and inventory.

## Cloning the Repository

To clone the repository, run:

```sh
git clone https://github.com/your-username/ECommerceApp.git
cd ECommerceApp
```

## Running the Solution Using Docker Compose

Ensure Docker and Docker Compose are installed on your machine.

### Docker Compose Setup

1. Make sure you are in the root directory of the cloned repository where the `docker-compose.yml` file is located.
2. To build and start all services defined in the `docker-compose.yml`, run:

    ```sh
    docker-compose up --build
    ```

3. To stop the services, run:

    ```sh
    docker-compose down
    ```

### Accessing Services

- **InventoryService**: [http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html)
- **OrderService**: [http://localhost:5002/swagger/index.html](http://localhost:5002/swagger/index.html)
- **RabbitMQ Management Interface**: [http://localhost:15672/](http://localhost:15672/)
- **RabbitMQ Credentials**: Username : `guest` , Password : `guest`
### Accessing SQL Server

To access the SQL Server used by the services, use the following credentials:

- **Server Name**: `localhost,1433`
- **Login**: `Sa`
- **Password**: `P@ssw0rd158`

## Configuration

- Ensure environment variables and related configurations are appropriately set for each service.
- Refer to individual service README files or documentation for specific configuration details.

## Logging

All services are configured to use `Microsoft.Extensions.Logging` for structured logging. Logs will be output to the console and can be viewed in the Docker logs or terminal output.

## Contributing

To contribute to this project, please fork the repository and submit a pull request. Ensure all new code is adequately tested and documented.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
