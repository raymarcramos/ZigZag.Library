# Zigzag API

This is the backend service for ZigZag Careers application. This document will guide you through the necessary steps to set up the local environment, including the database and API key configurations.

## Prerequisites

Before running the application, ensure that the following prerequisites are met:

1. **Local Database Setup**

    - There should be a local Microsoft SQL Server database named `ZigzagDB`.
    - Use the following connection string in your configuration:
      ```
      Server=localhost;Database=ZigzagDB;User Id=sa;Password=*******;TrustServerCertificate=True;
      ```
      > Note: Replace `*******` with your actual SQL Server password for the `sa` user.
    - The connection string can be customized in the `appsettings.json` file:

      ```json
      {
        "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Database=ZigzagDB;User Id=sa;Password=*******;TrustServerCertificate=True;"
        }
      }

2. **API Key Setup**

    The application requires an API key for external integrations. You can configure the API key either via:

    ### Option 1: User Secrets (for local development)

    Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to store the API key securely during development. Run the following commands in your project directory:

    ```bash
    dotnet user-secrets init
    dotnet user-secrets set "ApiKey" "raymarc-api-key"
    ```

    ### Option 2: `appsettings.json`

    Alternatively, you can add the API key directly to the `appsettings.json` file in the `ApiSettings` section like so:

    ```json
    {
      "ApiKey": "raymarc-api-key"
    }
    ```

## Getting Started

Follow these steps to run the application:

1. Ensure that your local SQL Server instance is running and the `ZigzagDB` database is created.
   
2. Configure the connection string and API key as described in the prerequisites.

3. **Entity Framework Code First Setup**:

    The database uses EF Core with a Code First approach. If you make changes to the models or `DbContext`, you need to run the migration script to update the database schema.

    To apply migrations, use the provided shell script (`run-migrations.sh`). You can execute it using Git Bash with the following command:
    
    ```bash
    bash run-migrations.sh SomethingUpdateDescription
    ```

    > **Note:** Replace `SomethingUpdateDescription` with a meaningful description of the migration (e.g., `AddNewFieldToBooks`).

