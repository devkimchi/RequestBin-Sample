using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

using RequestBin.FunctionApp.Models;

namespace RequestBin.FunctionApp.Helpers
{
    public interface IBinHelper
    {
        EntityId GetBin(Guid binId);
        EntityId GetBin(string binId);
        Task<BinItem> GetRequestAsync(HttpRequest req);
    }

    public class BinHelper : IBinHelper
    {
        public EntityId GetBin(Guid binId)
        {
            return this.GetBin(binId.ToString());
        }

        public EntityId GetBin(string binId)
        {
            var entityId = new EntityId(nameof(Actors.Bin), binId.ToString());

            return entityId;
        }

        public async Task<BinItem> GetRequestAsync(HttpRequest req)
        {
            var item = new BinItem();
            using (var reader = new StreamReader(req.Body))
            {
                item.Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
                item.Method = req.Method;
                item.Headers = req.Headers.AsEnumerable().ToDictionary(p => p.Key, p => string.Join(";", p.Value));
                item.Queries = req.Query.ToDictionary(p => p.Key, p => string.Join(";", p.Value));
                item.Body = await reader.ReadToEndAsync();
            }

            return item;
        }
    }
}