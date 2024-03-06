using System.Text.Json.Serialization;

namespace Crebitos.Application;

public class CreateTransactionDTO
{
    [JsonPropertyName("descricao")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("tipo")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("valor")]
    public int Value { get; set; }
}

