using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iot_temp_fn_app
{
    public static class history
    {
        [FunctionName("history")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           [CosmosDB(
                databaseName:"temptelemetry",
                containerName:"temptelemetry",
                Connection = "CosmosDbConnectionString",
                SqlQuery = "SELECT * FROM temptelemetry t WHERE timestampToDateTime(t._ts*1000) >= dateTimeAdd(\"dd\", -1, getCurrentDateTime()) ORDER BY t._ts ASC"
            )]IEnumerable<TemperatureTelemetry> tempModels,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<TemperatureTelemetryModel> ttm = new List<TemperatureTelemetryModel>();

            foreach (TemperatureTelemetry t in tempModels)
            {
                TemperatureTelemetryModel tm = new TemperatureTelemetryModel()
                {
                    Id = t.Id,
                    PartitionKey = t.PartitionKey,
                };

                byte[] data = Convert.FromBase64String(t.Body);
                tm.Temperature = System.Text.Encoding.UTF8.GetString(data);

                tm.Timestamp = DateTimeOffset.FromUnixTimeSeconds(t.Timestamp).UtcDateTime;

                ttm.Add(tm);
            }

            return new OkObjectResult(JsonConvert.SerializeObject(ttm));
        }
    }
}
