using System.Text.Json.Serialization;

namespace Crebitos.Application;

public class BalanceDTO
{
    [JsonPropertyName("limite")]
    public int Total { get; set; }

    [JsonPropertyName("limite")]
    public int Limit { get; set; }

    [JsonPropertyName("realizada_em")]
    public DateTime CreatedAt { get; set; }
}

public class TransactionDTO
{

    [JsonPropertyName("valor")]
    public int Value { get; set; }

    [JsonPropertyName("tipo")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("descricao")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("realizada_em")]
    public DateTime CreatedAt { get; set; }
}

public class GetStatementDTO
{
    [JsonPropertyName("saldo")]
    public BalanceDTO Balance { get; set; }

    [JsonPropertyName("ultimas_transacoes")]
    public TransactionDTO[] Transactions { get; set; }
}
