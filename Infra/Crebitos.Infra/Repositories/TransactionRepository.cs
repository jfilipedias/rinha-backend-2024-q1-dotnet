using System.Data.SqlClient;
using Dapper;
using Crebitos.Application;
using Crebitos.Domain;

namespace Crebitos.Infra;

public class TransactionRepository : ITransactionRepository
{
    private string connectionString;

    public TransactionRepository()
    {
        connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";
    }

    public Balance Save(Transaction transaction)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var tx = connection.BeginTransaction())
            {
                Balance balance;
                try
                {
                    balance = connection.QuerySingle<Balance>(
                        "SELECT c.debit_limit AS Limit, b.value AS Total FROM customers AS c INNER JOIN balances AS b ON b.customer_id = c.id WHERE c.id = @CustomerId FOR UPDATE;",
                        new { transaction.CustomerId }
                    );
                }
                catch (InvalidOperationException)
                {
                    throw new EntityNotFoundException();
                }

                var signedValue = transaction.Value;
                if (transaction.Type == "d")
                {
                    if (balance.Total - transaction.Value < -balance.Limit)
                    {
                        throw new InsufficientFundsException();
                    }

                    signedValue = -transaction.Value;
                }

                connection.Execute(
                    "UPDATE balances SET value = value + @Value WHERE customer_id = @CustomerId;",
                    new { Value = signedValue, transaction.CustomerId }
                );

                connection.Execute(
                    "INSERT INTO transactions (value, type, description, customer_id) VALUES (@Value, @Type, @Description, @CustomerId);",
                    new { transaction.Value, transaction.Type, transaction.Description, transaction.CustomerId }
                );

                tx.Commit();
                balance.Total += signedValue;

                return balance;
            }
        }
    }

    public List<Transaction> GetLatestByCustomerId(int customerId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var transactions = connection.Query<Transaction>(
                "SELECT type, value, description, created_at FROM transactions WHERE customer_id = @CustomerId ORDER BY created_at DESC LIMIT 10;",
                new { CustomerId = customerId }
            ).ToList();

            return transactions;
        }
    }
}
