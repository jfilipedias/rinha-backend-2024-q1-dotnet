using Npgsql;
using Crebitos.Application;
using Crebitos.Domain;

namespace Crebitos.Infra;

public class TransactionRepository : ITransactionRepository
{
    private NpgsqlConnection connection;

    public TransactionRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task<Balance> Save(Transaction transaction)
    {
        await connection.OpenAsync();
        await using var tx = await connection.BeginTransactionAsync();

        await using var getBalanceCommand = connection.CreateCommand();
        getBalanceCommand.CommandText = @"
            SELECT c.debit_limit AS Limit, b.value AS Total
            FROM customers AS c
            INNER JOIN balances AS b ON b.customer_id = c.id
            WHERE c.id = $1
            FOR UPDATE;";
        getBalanceCommand.Parameters.AddWithValue(transaction.CustomerId);
        await using var getBalanceReader = await getBalanceCommand.ExecuteReaderAsync();
        await getBalanceReader.ReadAsync();

        var balance = new Balance()
        {
            Limit = getBalanceReader.GetInt32(0),
            Total = getBalanceReader.GetInt32(1)
        };

        var signedValue = transaction.Value;
        if (transaction.Type == "d")
        {
            signedValue = -transaction.Value;

            if (balance.Total - transaction.Value < -balance.Limit)
            {
                throw new InsufficientFundsException();
            }
        }

        await using var updateBalanceCommand = connection.CreateCommand();
        updateBalanceCommand.CommandText = @"
            UPDATE balances 
            SET value = value + $1 
            WHERE customer_id = $2;";
        updateBalanceCommand.Parameters.AddWithValue(signedValue);
        updateBalanceCommand.Parameters.AddWithValue(transaction.CustomerId);
        await updateBalanceCommand.ExecuteNonQueryAsync();

        await using var insertTransactionCommand = connection.CreateCommand();
        insertTransactionCommand.CommandText = @"
            INSERT INTO transactions (value, type, description, customer_id) 
            VALUES ($1, $2, $3, $4);";
        insertTransactionCommand.Parameters.AddWithValue(transaction.Value);
        insertTransactionCommand.Parameters.AddWithValue(transaction.Type);
        insertTransactionCommand.Parameters.AddWithValue(transaction.Description);
        insertTransactionCommand.Parameters.AddWithValue(transaction.CustomerId);
        await insertTransactionCommand.ExecuteNonQueryAsync();

        await tx.CommitAsync();

        balance.Total += signedValue;
        return balance;
    }

    public async Task<List<Transaction>> GetLatestByCustomerId(int customerId)
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT type, value, description, created_at 
            FROM transactions 
            WHERE customer_id = $1 
            ORDER BY created_at DESC 
            LIMIT 10;";
        command.Parameters.AddWithValue(customerId);
        await using var reader = await command.ExecuteReaderAsync();

        var transactions = new List<Transaction>();
        while (await reader.ReadAsync())
        {
            transactions.Add(new Transaction()
            {
                Type = reader.GetString(0),
                Value = reader.GetInt32(1),
                Description = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            });
        }

        return transactions;
    }
}
