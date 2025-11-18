public record HistoricalResponse
(
    string From,
    string To,
    DateTime Start,
    DateTime End,
    List<RateCompact> Rates
);
