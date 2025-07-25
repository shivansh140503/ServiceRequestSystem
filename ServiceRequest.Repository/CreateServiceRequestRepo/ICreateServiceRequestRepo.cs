namespace ServiceRequest.Repository.CreateServiceRequestRepo;
public interface ICreateServiceRequestRepo
{
    Task<bool> CreateServiceRequestAsync(string title, string description, string status, string createdBy, CancellationToken cancellationToken);
}