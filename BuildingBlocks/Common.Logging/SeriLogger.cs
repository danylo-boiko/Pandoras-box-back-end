using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

        var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var environmentName = context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-");

        configuration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                IndexFormat = $"pandoras-box-log-{applicationName}-{environmentName}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
            .ReadFrom.Configuration(context.Configuration);
    };
}