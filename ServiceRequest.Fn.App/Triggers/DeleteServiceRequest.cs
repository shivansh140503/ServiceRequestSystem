using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using ServiceRequest.Models.ResponseModels;
using ServiceRequest.Repository.DeleteServiceRequestRepo;
using System.Net;

namespace ServiceRequest.Fn.App.Triggers;
public class DeleteServiceRequest
{
    private readonly IDeleteServiceRequestRepo _deleteServiceRequestRepo;

    public DeleteServiceRequest(IDeleteServiceRequestRepo deleteServiceRequestRepo)
    {
        _deleteServiceRequestRepo = deleteServiceRequestRepo;
    }

    [Function(nameof(DeleteServiceRequestById))]
    [OpenApiOperation(operationId: nameof(DeleteServiceRequestById), tags: new[] { "ServiceRequest" }, Summary = "Delete a service request")]
    [OpenApiParameter(name: "value", In = ParameterLocation.Query, Required = true, Type = typeof(int), Summary = "Service request ID to filter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "application/json", typeof(ApiResponse))]

    public async Task<IActionResult> DeleteServiceRequestById(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "service_request/delete")] HttpRequest httpRequest,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!int.TryParse(httpRequest.Query["value"].FirstOrDefault(), out int id))
            {
                return new BadRequestObjectResult(new ApiResponse("400", "Invalid or missing 'id' query parameter."));
            }

            var result = await _deleteServiceRequestRepo.DeleteServiceRequestAsync(id, cancellationToken);

            if (!result)
            {
                return new NotFoundObjectResult(new ApiResponse("404", $"No service request found with ID: {id}"));
            }

            return new OkObjectResult(new ApiResponse("200", $"Service request with ID {id} deleted successfully."));
        }
        catch (Exception ex)
        {
            // Log the exception here if using any logger (e.g., ILogger<DeleteServiceRequest>)
            return new ObjectResult(new ApiResponse("500", $"An error occurred: {ex.Message}"))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}