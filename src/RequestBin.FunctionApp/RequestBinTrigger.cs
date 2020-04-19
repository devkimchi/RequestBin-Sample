using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using RequestBin.FunctionApp.Actors;
using RequestBin.FunctionApp.Models;
using RequestBin.FunctionApp.Helpers;

namespace RequestBin.FunctionApp
{
    public class RequestBinTrigger
    {
        private readonly IBinHelper _helper;
        private readonly ILogger<RequestBinTrigger> _logger;

        public RequestBinTrigger(IBinHelper helper, ILogger<RequestBinTrigger> logger)
        {
            this._helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(CreateBin))]
        public async Task<IActionResult> CreateBin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="bins")] HttpRequest req,
            [DurableClient] IDurableClient client)
        {
            // Create bin reference
            var binId = Guid.NewGuid();
            var bin = this._helper.GetBin(binId);

            // Create bin
            await client.SignalEntityAsync<IBin>(bin, o => o.Add(null));

            // Create return.
            var nav = BinNavigation.Parse(binId, req.IsHttps, req.Host.ToString());

            var result = new JsonObjectContentResult(HttpStatusCode.Created, nav); 

            return result;
        }

        [FunctionName(nameof(AddHistory))]
        public async Task<IActionResult> AddHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "delete", Route="bins/{binId}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string binId)
        {
            var bin = this._helper.GetBin(binId);
            var history = await this._helper.GetRequestAsync(req);

            await client.SignalEntityAsync<IBin>(bin, o => o.Add(history));

            var nav = BinNavigation.Parse(binId, req.IsHttps, req.Host.ToString());

            var result = new JsonObjectContentResult(HttpStatusCode.Accepted, nav); 

            return result;
        }

        [FunctionName(nameof(GetHistory))]
        public async Task<IActionResult> GetHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="bins/{binId}/history")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string binId)
        {
            var bin = this._helper.GetBin(binId);

            var entity = await client.ReadEntityStateAsync<Bin>(bin);
            var payload = entity.EntityState;
            payload.Navigation = BinNavigation.Parse(binId, req.IsHttps, req.Host.ToString());

            var result = new JsonObjectContentResult(HttpStatusCode.OK, payload); 

            return result;
        }

        [FunctionName(nameof(ResetHistory))]
        public async Task<IActionResult> ResetHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="bins/{binId}/reset")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string binId)
        {
            var bin = this._helper.GetBin(binId);

            await client.SignalEntityAsync<IBin>(bin, o => o.Reset());

            var nav = BinNavigation.Parse(binId, req.IsHttps, req.Host.ToString());

            var result = new JsonObjectContentResult(HttpStatusCode.Accepted, nav); 

            return result;
        }

        [FunctionName(nameof(PurgeBin))]
        public async Task<IActionResult> PurgeBin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="bins/{binId}/purge")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string binId)
        {
            var bin = this._helper.GetBin(binId);

            await client.PurgeInstanceHistoryAsync($"@{bin.EntityName}@{bin.EntityKey}");

            var result = new NoContentResult();

            return result;
        }
    }
}