using System.Net.Http.Formatting;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using RequestBin.FunctionApp.Helpers;

[assembly: FunctionsStartup(typeof(RequestBin.FunctionApp.StartUp))]
namespace RequestBin.FunctionApp
{
    /// <summary>
    /// This represents the entity for the IoC container.
    /// </summary>
    public class StartUp : FunctionsStartup
    {
        private const string StorageConnectionStringKey = "AzureWebJobsStorage";

        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureHttpClient(builder.Services);
            this.ConfigureJsonSerialiser(builder.Services);
            this.ConfigureHelpers(builder.Services);
        }

        private void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        private void ConfigureJsonSerialiser(IServiceCollection services)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                Converters = { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            var formatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings = settings
            };

            services.AddSingleton<JsonSerializerSettings>(settings);
            services.AddSingleton<JsonMediaTypeFormatter>(formatter);
        }

        private void ConfigureHelpers(IServiceCollection services)
        {
            services.AddTransient<IBinHelper, BinHelper>();
        }
    }
}