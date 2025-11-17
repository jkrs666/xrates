public record Rate
(
    DateTime Timestamp,
    String Base,
    String Quote,
    decimal Value
)
{
    public int Id { get; set; }
};
