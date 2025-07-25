using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using ServiceRequest.Fn.App.Configuration.FeatureExtensions;
using ServiceRequest.Repository.CreateServiceRequestRepo;
using ServiceRequest.Repository.GetServiceRequestRepo;
using ServiceRequest.Repository.UpdateServiceRequestRepo;
using ServiceRequest.Repository.DeleteServiceRequestRepo;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") ?? "production";
        var rootPath = context.HostingEnvironment.ContentRootPath;

        configBuilder.SetBasePath(rootPath)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables()
                     .AddUserSecrets(typeof(Program).Assembly, optional: true);

    })
    .ConfigureServices((context, services) =>
    {
        // Configuration
        var configuration = context.Configuration;

        services.AddOptions<ConfigurationSettings>()
                .Configure<IConfiguration>((settings, config) => config.Bind(settings));

        // ✅ Use feature extension to register SQLDatabaseSettings
        services.AddSqlDbFeature(configuration);

        // ✅ Register Repository for dependency injection
        services.AddScoped<ICreateServiceRequestRepo, CreateServiceRequestRepo>();
        services.AddScoped<IGetServiceRequestRepo, GetServiceRequestRepo>();
        services.AddScoped<IUpdateServiceRequestRepo, UpdateServiceRequestRepo>();
        services.AddScoped<IDeleteServiceRequestRepo, DeleteServiceRequestRepo>();

        // Optional: Application Insights and telemetry
        services.ConfigureFunctionsApplicationInsights();
        services.AddApplicationInsightsTelemetryWorkerService();

        //✅ Configure JSON serialization options for ignoring null values
        services.Configure<JsonSerializerOptions>(options =>
        {
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Optional: Use camelCase for JSON
        });
    })
    .Build();

host.Run();
