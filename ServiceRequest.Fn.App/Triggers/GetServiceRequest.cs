using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using ServiceRequest.Models.ServiceRequestModels;
using ServiceRequest.Repository.GetServiceRequestRepo;
using System.Net;

namespace ServiceRequest.Fn.App.Triggers;

public class GetServiceRequest
{
    private readonly IGetServiceRequestRepo _getServiceRequestRepo;
    public GetServiceRequest(IGetServiceRequestRepo getServiceRequestRepo)
    {
        _getServiceRequestRepo = getServiceRequestRepo;
    }

    [Function(nameof(GetServiceRequestById))]
    [OpenApiOperation(operationId: nameof(GetServiceRequestById), tags: new[] { "ServiceRequest" }, Summary = "Get service request(s)")]
    [OpenApiParameter(name: "value", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "Optional service request ID to filter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IEnumerable<ServiceRequestModel>))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", typeof(string))]
    public async Task<IActionResult> GetServiceRequestById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "service_request/get")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var value = req.Query["value"].FirstOrDefault();
        int? id = null;

        if (!string.IsNullOrWhiteSpace(value))
        {
            if (!int.TryParse(value, out var parsedId))
                return new BadRequestObjectResult("Invalid 'value'. It must be a valid integer.");
            id = parsedId;
        }

        var result = await _getServiceRequestRepo.GetServiceRequestsAsync(id, cancellationToken);

        if (result is null || !result.Any())
        {
            if (id.HasValue)
                return new NotFoundObjectResult($"No service request found with ID: {id.Value}");
            else
                return new NotFoundObjectResult("No service requests found.");
        }

        return new OkObjectResult(result);
    }
}
