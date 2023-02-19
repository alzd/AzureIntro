namespace EtherChecker;

public class Currencies
{
    public const string DefaultCurrency = "USD";
}

public record Currency
{
    public string Code { get; init; } = string.Empty;
}