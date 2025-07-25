using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceRequest.Models.SqlObjectModels;
using System.Data;

namespace ServiceRequest.Fn.App.Configuration.FeatureExtensions;

public static class SqlDbFeatureExtension
{
    public static IServiceCollection AddSqlDbFeature(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind stored proc/view/query names to strongly-typed settings
        services.Configure<SqlObjectSettings>(configuration.GetSection("SqlDbObjects"));

        // Read SQL DB connection string from local.settings.json
        var connectionString = configuration["SqlDbConnectionString"]
            ?? throw new ArgumentNullException("Missing 'SqlDbConnectionString' in local settings.");

        // Inject SQL connection (transient)
        services.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString));

        return services;
    }
}
