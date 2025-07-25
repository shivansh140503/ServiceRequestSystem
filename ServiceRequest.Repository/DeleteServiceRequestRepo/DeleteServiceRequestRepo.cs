using Microsoft.Data.SqlClient;
using System.Data;

namespace ServiceRequest.Repository.DeleteServiceRequestRepo;
public class DeleteServiceRequestRepo : IDeleteServiceRequestRepo
{
    private readonly IDbConnection _connection;

    public DeleteServiceRequestRepo(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> DeleteServiceRequestAsync(int id, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
            throw new InvalidOperationException("Expected SqlConnection");

        const string query = @"
                DELETE FROM ServiceRequests
                WHERE Id = @Id";

        using var command = new SqlCommand(query, sqlConnection);
        command.Parameters.AddWithValue("@Id", id);

        await sqlConnection.OpenAsync(cancellationToken);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        await sqlConnection.CloseAsync();

        return rowsAffected > 0;
    }
}
