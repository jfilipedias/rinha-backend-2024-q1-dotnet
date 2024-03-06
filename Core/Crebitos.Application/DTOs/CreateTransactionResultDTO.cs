using System.Text.Json.Serialization;

namespace Crebitos.Application;

public class CreateTransactionResultDTO
{
    [JsonPropertyName("saldo")]
    public int Balance { get; set; }
    [JsonPropertyName("limite")]
    public int Limit { get; set; }
}
