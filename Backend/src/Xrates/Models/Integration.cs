public record Integration
(
    string Name,
    string Url,
    int FreqSeconds,
    int Priority,
    bool Enabled
//TODO: encrypt
//public string ApiKey { get; set; }
);
