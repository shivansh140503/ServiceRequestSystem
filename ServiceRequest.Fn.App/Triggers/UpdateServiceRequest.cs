using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using ServiceRequest.Models.ResponseModels;
using ServiceRequest.Models.ServiceRequestModels;
using ServiceRequest.Repository.UpdateServiceRequestRepo;
using System.Net;

namespace ServiceRequest.Fn.App.Triggers;

public class UpdateServiceRequest
{
    public readonly IUpdateServiceRequestRepo _updateServiceRequestRepo;

    public UpdateServiceRequest(IUpdateServiceRequestRepo updateServiceRequestRepo)
    {
        _updateServiceRequestRepo = updateServiceRequestRepo;
    }

    [Function(nameof(PutServiceRequest))]
    [OpenApiOperation(operationId: nameof(PutServiceRequest), tags: new[] { "ServiceRequest" }, Summary = "Update a service request")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(int), Summary = "Id of the request")]
    [OpenApiRequestBody("application/json", typeof(UpdateServiceRequestModel), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(ApiResponse))]
    [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "application/json", typeof(ApiResponse))]

    public async Task<HttpResponseData> PutServiceRequest(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "service_request/update")] HttpRequestData httpRequest,
    CancellationToken cancellationToken
        )
    {
        try
        {
            if (!int.TryParse(System.Web.HttpUtility.ParseQueryString(httpRequest.Url.Query)["id"], out int id))
            {
                var error = httpRequest.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(new ApiResponse("400", "Invalid or missing 'id' query parameter."));
                return error;
            }

            var body = await httpRequest.ReadFromJsonAsync<UpdateServiceRequestModel>();

            if (body is null || string.IsNullOrWhiteSpace(body.Description) || string.IsNullOrWhiteSpace(body.Status))
            {
                var errorResponse = httpRequest.CreateResponse(HttpStatusCode.BadRequest);
                await errorResponse.WriteAsJsonAsync(new ApiResponse("400", "Missing required fields in body."));
                return errorResponse;
            }

            bool success = await _updateServiceRequestRepo.UpdateServiceRequestAsync(id, body.Description, body.Status, cancellationToken);

            var response = httpRequest.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new ApiResponse("200", "Service request updated successfully."));
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = httpRequest.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new ApiResponse("500", $"Unexpected error: {ex.Message}"));
            return errorResponse;
        }
    }
}