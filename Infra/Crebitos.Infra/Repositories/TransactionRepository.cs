using System.Data.SqlClient;
using Dapper;
using Crebitos.Domain;

namespace Crebitos.Infra;

public class TransactionRepository : ITransactionRepository
{
    public Balance Save(Transaction transaction)
    {
        Balance balance;
        using (var connection = new SqlConnection())
        {
            connection.Open();

            using (var tx = connection.BeginTransaction())
            {
                balance = connection.QuerySingle<Balance>(
                    "SELECT c.debit_limit AS Limit, b.value AS Total FROM customers AS c INNER JOIN balances AS b ON b.customer_id = c.id WHERE c.id = @CustomerId FOR UPDATE;",
                    new { CustomerId = transaction.CustomerId }
                );

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
                    "UPDATE balances SET value = value + @Value WHERE customer_id = @CustomerId",
                    new { Value = signedValue, CustomerId = transaction.CustomerId }
                );

                connection.Execute(
                    "INSERT INTO transactions (value, type, description, customer_id) VALUES (@Value, @Type, @Description, @CustomerId)",
                    new { Value = transaction.Value, Type = transaction.Type, Description = transaction.Description, CustomerId = transaction.CustomerId }
                );

                tx.Commit();
                balance.Total += signedValue;
            }
        }

        return balance;
    }

    public Transaction[] GetLatestByCustomerId(int customerId)
    {
        throw new NotImplementedException();
    }
}
