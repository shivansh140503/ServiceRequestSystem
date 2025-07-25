using ServiceRequest.Models.ServiceRequestModels;

namespace ServiceRequest.Repository.GetServiceRequestRepo;
public interface IGetServiceRequestRepo
{
    Task<IEnumerable<ServiceRequestModel>> GetServiceRequestsAsync(int? id, CancellationToken cancellationToken);
}
