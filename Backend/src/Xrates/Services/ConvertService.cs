public class ConvertService
{
    private readonly ILogger<ConvertService> _logger;

    public ConvertService(ILogger<ConvertService> logger)
    {
        _logger = logger;
    }

    public decimal CalculateConversionRate(decimal from, decimal to)
    {
        return (1 / from) * to;
    }
}
