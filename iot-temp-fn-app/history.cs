using System;
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
        // Manual subtraction of 24 hours from current timestamp instead of using built-in functions due to performance gains.
        //
        // Query explanation:
        // - 'GetCurrentTimestamp()/1000' to convert miliseconds to seconds and match the c._ts property values.
        // - '86400' denotes 24 hours in seconds
        private const string HistoryQuery = "SELECT c.Body, c._ts FROM c WHERE c._ts >= GetCurrentTimestamp()/1000 - 86400";

        [FunctionName("history")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           [CosmosDB(
                databaseName:"temptelemetry",
                containerName:"temptelemetry",
                Connection = "CosmosDbConnectionString",
                SqlQuery = HistoryQuery
            )]IEnumerable<TemperatureTelemetry> tempModels,
           ILogger log)
        {
            List<TemperatureTelemetryModel> ttm = new List<TemperatureTelemetryModel>();

            foreach (TemperatureTelemetry t in tempModels)
            {
                TemperatureTelemetryModel tm = new TemperatureTelemetryModel();

                byte[] data = Convert.FromBase64String(t.Body);
                tm.Temperature = System.Text.Encoding.UTF8.GetString(data);

                tm.Timestamp = DateTimeOffset.FromUnixTimeSeconds(t.Timestamp).UtcDateTime;

                ttm.Add(tm);
            }

            return new OkObjectResult(JsonConvert.SerializeObject(ttm));
        }
    }
}
