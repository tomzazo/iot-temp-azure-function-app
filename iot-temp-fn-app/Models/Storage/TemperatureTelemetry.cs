using System;
using Newtonsoft.Json;

namespace iot_temp_fn_app
{
    public class TemperatureTelemetry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty("Body")]
        public string Body { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}