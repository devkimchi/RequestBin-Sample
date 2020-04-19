using System.Collections.Generic;

using Newtonsoft.Json;

namespace RequestBin.FunctionApp.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BinItem
    {
        [JsonProperty("timestamp")]
        public virtual string Timestamp { get; set; }

        [JsonProperty("method")]
        public virtual string Method { get; set; }

        [JsonProperty("headers")]
        public virtual Dictionary<string, string> Headers { get; set; }

        [JsonProperty("queries")]
        public virtual Dictionary<string, string> Queries { get; set; }

        [JsonProperty("body")]
        public virtual string Body { get; set; }
    }
}