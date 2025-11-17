public record HistoricalResponse
(
    string From,
    string To,
    DateTime Start,
    DateTime End,
    IEnumerable<HistoricalRate> Rates
);
