provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "iot-temp" {
  name     = "iot-temp"
  location = "North Europe"
}

resource "azurerm_storage_account" "iottempstorage" {
  name                     = "iottempstorage"
  resource_group_name      = azurerm_resource_group.iot-temp.name
  location                 = azurerm_resource_group.iot-temp.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_service_plan" "iot-temp-service-plan" {
  name                = "iot-temp-service-plan"
  resource_group_name = azurerm_resource_group.iot-temp.name
  location            = azurerm_resource_group.iot-temp.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "iot-temp-fn-app" {
  name                = "iot-temp-fn-app"
  resource_group_name = azurerm_resource_group.iot-temp.name
  location            = azurerm_resource_group.iot-temp.location

  storage_account_name       = azurerm_storage_account.iottempstorage.name
  storage_account_access_key = azurerm_storage_account.iottempstorage.primary_access_key
  service_plan_id            = azurerm_service_plan.iot-temp-service-plan.id

  site_config {
    application_stack {
      dotnet_version = "6.0"
      use_dotnet_isolated_runtime = "false"
    }
  }
}
