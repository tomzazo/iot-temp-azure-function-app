using System;
using Newtonsoft.Json;

namespace iot_temp_fn_app
{
    public class TemperatureTelemetryModel
    {
        [JsonProperty("temperature")]
        public string Temperature { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}