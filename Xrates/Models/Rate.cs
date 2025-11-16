// Store only with USD as base
public record Rate
(
    int Id,
    DateTime Timestamp,
    String Currency,
    decimal Value
);
