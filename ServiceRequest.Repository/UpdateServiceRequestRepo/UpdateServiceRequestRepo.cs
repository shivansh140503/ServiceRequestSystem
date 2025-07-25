using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ServiceRequest.Models.SqlObjectModels;
using System.Data;

namespace ServiceRequest.Repository.UpdateServiceRequestRepo;
public class UpdateServiceRequestRepo : IUpdateServiceRequestRepo
{
    private readonly IDbConnection _connection;
    private readonly SqlObjectSettings _dbObjects;
    public UpdateServiceRequestRepo(IDbConnection connection, IOptions<SqlObjectSettings> dbOptions)
    {
        _connection = connection;
        _dbObjects = dbOptions.Value;
    }

    public async Task<bool> UpdateServiceRequestAsync(int id, string description, string status, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
            throw new InvalidOperationException("Expected SqlConnection");

        if (string.IsNullOrWhiteSpace(_dbObjects.UpdateServiceRequestProc))
            throw new InvalidOperationException("Stored procedure name is missing or empty.");

        using var command = new SqlCommand(_dbObjects.UpdateServiceRequestProc, sqlConnection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Description", description);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@id", id);

        await sqlConnection.OpenAsync(cancellationToken);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0;
    }
}
