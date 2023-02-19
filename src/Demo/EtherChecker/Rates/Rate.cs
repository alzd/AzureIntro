namespace EtherChecker.Rates;

public sealed record Rate(string BaseCurrencyCode, string TargetCurrencyCode, decimal Value, DateTimeOffset? Timestamp);

