// Store only with USD as base
public record Rate
(
    DateTime Timestamp,
    String Currency,
    decimal Value
)
{
    public int Id { get; set; }
};
