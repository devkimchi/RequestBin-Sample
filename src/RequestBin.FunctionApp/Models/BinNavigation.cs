using System;

using Newtonsoft.Json;

namespace RequestBin.FunctionApp.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BinNavigation
    {
        [JsonProperty("bin")]
        public virtual string BinUrl { get; set; }

        [JsonProperty("history")]
        public virtual string BinHistoryUrl { get; set; }

        [JsonProperty("reset")]
        public virtual string BinResetUrl { get; set; }

        [JsonProperty("purge")]
        public virtual string BinPurgeUrl { get; set; }

        public static BinNavigation Parse(Guid binId, bool isHttps = false, string host = "http://localhost:7071/api")
        {
            return Parse(binId.ToString(), isHttps, host);
        }

        public static BinNavigation Parse(string binId, bool isHttps = false, string host = "http://localhost:7071/api")
        {
            var nav = new BinNavigation()
            {
                BinUrl = $"http{(isHttps ? "s" : string.Empty)}://{host}/bins/{binId}",
                BinHistoryUrl = $"http{(isHttps ? "s" : string.Empty)}://{host}/bins/{binId}/history",
                BinResetUrl = $"http{(isHttps ? "s" : string.Empty)}://{host}/bins/{binId}/reset",
                BinPurgeUrl = $"http{(isHttps ? "s" : string.Empty)}://{host}/bins/{binId}/purge",
            };

            return nav;
        }
    }
}