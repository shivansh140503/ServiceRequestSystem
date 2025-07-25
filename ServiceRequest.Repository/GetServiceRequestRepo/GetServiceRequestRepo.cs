using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ServiceRequest.Models.ServiceRequestModels;
using ServiceRequest.Models.SqlObjectModels;
using System.Data;

namespace ServiceRequest.Repository.GetServiceRequestRepo;
public class GetServiceRequestRepo : IGetServiceRequestRepo
{
    private readonly IDbConnection _connection;

    public GetServiceRequestRepo(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<ServiceRequestModel>> GetServiceRequestsAsync(int? id, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
            throw new InvalidOperationException("Expected SqlConnection");

        var query = @"
                SELECT Id, Title, Description, Status, CreatedBy, CreatedDate
                FROM ServiceRequests
                WHERE (@Id IS NULL OR Id = @Id)";


        using var command = new SqlCommand(query, sqlConnection);
        command.Parameters.AddWithValue("@Id", id.HasValue ? id.Value : DBNull.Value);

        await sqlConnection.OpenAsync(cancellationToken);
        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var dataTable = new DataTable();
        dataTable.Load(reader);

        var results = dataTable.AsEnumerable()
            .Select(row => new ServiceRequestModel(
                row.Field<int>("Id"),
                row["Title"]?.ToString() ?? string.Empty,
                row["Description"]?.ToString() ?? string.Empty,
                row["Status"]?.ToString() ?? string.Empty,
                row["CreatedBy"]?.ToString() ?? string.Empty,
                row.Field<DateTime?>("CreatedDate") ?? DateTime.MinValue
                ))
            .ToList();

        return results;
    }
}
