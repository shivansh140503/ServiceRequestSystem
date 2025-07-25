using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ServiceRequest.Fn.App.Configuration;

public class DevOpsFunctions
{
    public const string DevOpsTag = "DevOps";
    private readonly IMemoryCache _cache;
    private readonly ILogger<DevOpsFunctions> _logger;

    public DevOpsFunctions(
    IMemoryCache cache,
    ILogger<DevOpsFunctions> logger
)
    {
        _cache = cache;
        _logger = logger;
    }

    [Function(nameof(RedirectToSwaggerUi))]
    public IActionResult RedirectToSwaggerUi(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "{default:maxlength(0)?}"
        )] HttpRequest req
    ) => new RedirectResult("/swagger/ui/");

    #region Icons
    [Function(nameof(Favicon))]
    public IActionResult Favicon(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "favicon.ico")] HttpRequest req
    ) => GetEmbeddedResourceIcon(req, "favicon.ico", "image/x-icon");

    // Add in swagger ui favicons until natively (and hopefully better) supported by extension (https://github.com/Azure/azure-functions-openapi-extension/issues/147)
    [Function(nameof(SwaggerIcon32))]
    public IActionResult SwaggerIcon32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/favicon-32x32.png")] HttpRequest req
    ) => GetEmbeddedResourceIcon(req, "favicon-32x32.png", "image/png");

    [Function(nameof(SwaggerUiIcon32))]
    public IActionResult SwaggerUiIcon32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui/favicon-32x32.png")] HttpRequest req
    ) => GetEmbeddedResourceIcon(req, "favicon-32x32.png", "image/png");

    [Function(nameof(SwaggerIcon16))]
    public IActionResult SwaggerIcon16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/favicon-16x16.png")] HttpRequest req
    ) => GetEmbeddedResourceIcon(req, "favicon-16x16.png", "image/png");

    [Function(nameof(SwaggerUiIcon16))]
    public IActionResult SwaggerUiIcon16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui/favicon-16x16.png")] HttpRequest req
    ) => GetEmbeddedResourceIcon(req, "favicon-16x16.png", "image/png");

    private static IActionResult GetEmbeddedResourceIcon(HttpRequest req, string iconName, string mimeType)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourcePath = $"{nameof(ServiceRequest)}.{nameof(Fn)}.{nameof(App)}.dist.{iconName}";

        Console.WriteLine($"[INFO] Attempting to load embedded resource: {resourcePath}");

        // Attempt to load embedded resource
        var stream = assembly.GetManifestResourceStream(resourcePath);

        if (stream == null)
        {
            // Log available embedded resources for debugging
            var availableResources = assembly.GetManifestResourceNames();
            Console.WriteLine($"[ERROR] Embedded resource '{resourcePath}' not found.");
            Console.WriteLine("Available embedded resources:");
            foreach (var resource in availableResources)
            {
                Console.WriteLine($"  - {resource}");
            }

            // Attempt to load from the file system as a fallback
            string currentDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDir, "dist", iconName);

            Console.WriteLine($"[INFO] Checking physical file path: {filePath}");

            if (System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"[SUCCESS] Loading favicon from: {filePath}");
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                Console.WriteLine($"[ERROR] File not found at path: {filePath}");
                return new NotFoundResult();
            }
        }
        else
        {
            Console.WriteLine($"[SUCCESS] Loaded embedded resource: {resourcePath}");
        }

        // Set caching headers
        req.HttpContext.Response.Headers.Append("Cache-Control", $"max-age={60 * 60 * 24}");
        return new FileStreamResult(stream, mimeType);
    }
    #endregion
}
