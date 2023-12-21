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
    public static class latest
    {
        [FunctionName("latest")]
        public static IActionResult Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           [CosmosDB(
                databaseName:"temptelemetry",
                containerName:"temptelemetry",
                Connection = "CosmosDbConnectionString",
                SqlQuery = "SELECT TOP 1 * FROM temptelemetry t ORDER BY t._ts DESC"
            )]IEnumerable<TemperatureTelemetry> tempModels,
           ILogger log)
        {
            foreach (TemperatureTelemetry t in tempModels)
            {
                TemperatureTelemetryModel tm = new TemperatureTelemetryModel();

                byte[] data = Convert.FromBase64String(t.Body);
                tm.Temperature = System.Text.Encoding.UTF8.GetString(data);

                tm.Timestamp = DateTimeOffset.FromUnixTimeSeconds(t.Timestamp).UtcDateTime;

                return new OkObjectResult(JsonConvert.SerializeObject(tm));
            }

            return new BadRequestObjectResult("Error fetching data from DB.");
        }
    }
}
