﻿using Npgsql;
using Crebitos.Application;
using Crebitos.Domain;

namespace Crebitos.Infra;

public class BalanceRepository : IBalanceRepository
{
    private NpgsqlConnection connection;

    public BalanceRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task<Balance> GetByCustomerId(int customerId)
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT c.debit_limit AS limit, b.value AS total 
            FROM customers AS c 
            LEFT JOIN balances AS b ON b.customer_id = c.id 
            WHERE c.id = $1;";
        command.Parameters.AddWithValue(customerId);

        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var balance = new Balance()
        {
            Limit = reader.GetInt32(0),
            Total = reader.GetInt32(1)
        };

        return balance;
    }
}
