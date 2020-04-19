using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

using Newtonsoft.Json;

using RequestBin.FunctionApp.Models;

namespace RequestBin.FunctionApp.Actors
{
    public interface IBin
    {
        void Add(BinItem item);
        void Reset();
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Bin : IBin
    {
        [JsonProperty("history")]
        public virtual List<BinItem> History { get; set; } = new List<BinItem>();

        [JsonProperty("navigation")]
        public virtual BinNavigation Navigation { get; set; }

        public void Add(BinItem item)
        {
            if (item == null)
            {
                return;
            }

            this.History.Insert(0, item);
        }

        public void Reset()
        {
            this.History.Clear();
        }

        [FunctionName(nameof(Bin))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<Bin>();
    }
}