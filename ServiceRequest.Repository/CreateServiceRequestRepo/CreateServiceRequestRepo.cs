using Microsoft.Data.SqlClient;
using ServiceRequest.Models.SqlObjectModels;
using System.Data;
using Microsoft.Extensions.Options;

namespace ServiceRequest.Repository.CreateServiceRequestRepo;

public class CreateServiceRequestRepo : ICreateServiceRequestRepo
{
    private readonly IDbConnection _connection;
    private readonly SqlObjectSettings _dbObjects;

    public CreateServiceRequestRepo(IDbConnection connection, IOptions<SqlObjectSettings> dbOptions)
    {
        _connection = connection;
        _dbObjects = dbOptions.Value;
    }
    public async Task<bool> CreateServiceRequestAsync(string title, string description, string status, string createdBy, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
            throw new InvalidOperationException("Expected SqlConnection");

        if (string.IsNullOrWhiteSpace(_dbObjects.CreateServiceRequestProc))
            throw new InvalidOperationException("Stored procedure name is missing or empty.");

        using var command = new SqlCommand(_dbObjects.CreateServiceRequestProc, sqlConnection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Description", description);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@CreatedBy", createdBy);

        await sqlConnection.OpenAsync(cancellationToken);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0;
    }
}