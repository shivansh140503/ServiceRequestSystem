namespace ServiceRequest.Repository.UpdateServiceRequestRepo;
public interface IUpdateServiceRequestRepo
{
    Task<bool> UpdateServiceRequestAsync(int id, string description, string status, CancellationToken cancellationToken);
}