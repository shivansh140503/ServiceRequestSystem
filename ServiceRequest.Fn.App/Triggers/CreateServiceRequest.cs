using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using ServiceRequest.Models.ResponseModels;
using ServiceRequest.Repository.CreateServiceRequestRepo;
using System.Net;

namespace ServiceRequest.Fn.App.Triggers;

public class CreateServiceRequest
{
    public readonly ICreateServiceRequestRepo _createServiceRequestRepo;

    public CreateServiceRequest(ICreateServiceRequestRepo createServiceRequestRepo)
    {
        _createServiceRequestRepo = createServiceRequestRepo;
    }

    [Function(nameof(PostServiceRequest))]
    [OpenApiOperation(operationId: nameof(PostServiceRequest), tags: new[] { "ServiceRequest" }, Summary = "Create a service request")]

    [OpenApiParameter(name: "title", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Title of the request")]
    [OpenApiParameter(name: "description", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Description of the request")]
    [OpenApiParameter(name: "status", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Status of the request")]
    [OpenApiParameter(name: "createdBy", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Created By of the request")]

    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "application/json", typeof(ApiResponse))]

    public async Task<IActionResult> PostServiceRequest(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "service_request/create")] HttpRequest httpRequest,
        CancellationToken cancellationToken
        )
    {
        string? title = httpRequest.Query["title"].FirstOrDefault();
        string? description = httpRequest.Query["description"].FirstOrDefault();
        string? status = httpRequest.Query["status"].FirstOrDefault();
        string? createdBy = httpRequest.Query["createdBy"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || 
            string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(createdBy))
        {
            var errorResponse = new ApiResponse("400", "Missing one or more required query parameters.");
            return new BadRequestObjectResult(errorResponse);
        }

        try
        {
            bool success = await _createServiceRequestRepo.CreateServiceRequestAsync(title, description, status, createdBy, cancellationToken);

            if (success)
                return new OkObjectResult(new ApiResponse("200", "Service Request Created Successfully"));
            else
                return new ObjectResult(new ApiResponse("500", "Failed to create service request")) { StatusCode = 500 };
        }
        catch (Exception ex)
        {
            return new ObjectResult(new ApiResponse("500", $"Error: {ex.Message}")) { StatusCode = 500 };
        }
    }
}