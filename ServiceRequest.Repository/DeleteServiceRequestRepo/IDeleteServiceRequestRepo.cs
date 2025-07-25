namespace ServiceRequest.Repository.DeleteServiceRequestRepo;
public interface IDeleteServiceRequestRepo
{
    Task<bool> DeleteServiceRequestAsync(int id, CancellationToken cancellationToken);
}
