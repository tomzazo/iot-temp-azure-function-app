using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace iot_temp_fn_app
{
    public static class latest_temperature
    {
        [FunctionName("latest_temperature")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName:"temptelemetry",
                containerName:"temptelemetry",
                Connection = "CosmosDbConnectionString",
                SqlQuery = "SELECT TOP 1 * FROM temptelemetry t ORDER BY t._ts DESC"
            )]IEnumerable<TemperatureTelemetry> tempModels,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            foreach (TemperatureTelemetry t in tempModels)
            {
                TemperatureTelemetryModel tm = new TemperatureTelemetryModel() {
                    Id = t.Id,
                    PartitionKey = t.PartitionKey,
                };

                byte[] data = Convert.FromBase64String(t.Body);
                tm.Temperature = System.Text.Encoding.UTF8.GetString(data);

                tm.Timestamp = DateTimeOffset.FromUnixTimeSeconds(t.Timestamp).UtcDateTime;

                return new OkObjectResult(JsonConvert.SerializeObject(tm));
            }

            return new BadRequestObjectResult("Error fetching data from DB.");
        }
    }
}
