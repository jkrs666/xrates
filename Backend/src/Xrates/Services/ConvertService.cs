public class ConvertService
{
    private readonly ILogger<ExternalApiService> _logger;

    public ConvertService(ILogger<ExternalApiService> logger)
    {
        _logger = logger;
    }

    public decimal CalculateConversionRate(decimal from, decimal to)
    {
        return (1 / from) * to;
    }

    public decimal Convert(decimal rate, decimal amount)
    {
        return rate * amount;
    }

    public decimal Convert(decimal from, decimal to, decimal amount)
    {
        return CalculateConversionRate(from, to) * amount;
    }

}
