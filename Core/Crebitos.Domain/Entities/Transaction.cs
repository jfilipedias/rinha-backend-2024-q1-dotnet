namespace Crebitos.Domain;

public class Transaction
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime CreatedAt { get; set; }
}
