public record Integration
(
    string Name,
    string Url,
    int FreqSeconds,
    int Priority,
    bool Enabled,
    string BaseCurrency,
    string timestampJsonField,
    string ratesJsonField
//TODO: encrypt
//public string ApiKey { get; set; }
);
