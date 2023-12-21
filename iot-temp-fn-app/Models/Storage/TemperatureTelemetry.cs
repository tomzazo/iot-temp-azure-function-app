using Newtonsoft.Json;

namespace iot_temp_fn_app
{
    public class TemperatureTelemetry
    {
        [JsonProperty("Body")]
        public string Body { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}