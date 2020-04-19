using System.Net;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace RequestBin.FunctionApp.Models
{
    public class JsonObjectContentResult : ContentResult
    {
        public JsonObjectContentResult(HttpStatusCode httpStatusCode, object payload)
        {
            this.Content = JsonConvert.SerializeObject(payload, Formatting.Indented);
            this.ContentType = "application/json";
            this.StatusCode = (int)httpStatusCode;
        }
    }
}