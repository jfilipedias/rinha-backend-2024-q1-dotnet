using System.Data.SqlClient;
using Dapper;
using Crebitos.Application;
using Crebitos.Domain;

namespace Crebitos.Infra;

public class BalanceRepository : IBalanceRepository
{
    private string connectionString;

    public BalanceRepository()
    {
        connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";
    }

    public Balance GetByCustomerId(int customerId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var balance = connection.QuerySingle<Balance>(
                "SELECT c.debit_limit AS limit, b.value AS total, NOW() AS created_at FROM customers AS c LEFT JOIN balances AS b ON b.customer_id = c.id WHERE c.id = @CustomerId",
                new { CustomerId = customerId }
            );

            return balance;
        }
    }
}
