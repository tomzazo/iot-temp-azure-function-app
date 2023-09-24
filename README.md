## Temperature telemetry Azure function app

An example of an Azure function app which reads data from CosmosDB containing temperature readings.

### Pre-requisites

- Terraform
  - Connected to Azure
- Dotnet SDK 6.0

### Required secrets

Once the function app is created on Azure, go under `Configuration` and insert a new connection string key-value pair with `CosmosDbConnectionString` as name and the CosmosDB connection string as value.

### Usage

Two endpoints are exposed from the function app:

- `/api/latest` which returns the latest recorded temperature reading
- `/api/history` which returns recorded temperature readings for the past 24 hours
