using System.Globalization;
using Azure;
using Azure.Data.Tables;

namespace EtherChecker.Rates;

internal class RateStorage : IRateStorage
{
    private readonly TableClient _table;

    public RateStorage(TableServiceClient client)
    {
        _table = client.GetTableClient("rates");
        _table.CreateIfNotExists();
    }
    
    public Rate? GetRate(string currency)
    {
        return _table.Query<RateDo>(rate =>
            rate.TargetCurrencyCode.Equals(currency, StringComparison.InvariantCultureIgnoreCase)).MaxBy(rate =>rate.Timestamp);
    }

    public async Task SaveRatesAsync(IList<Rate> rates)
    {
        foreach (var rate in rates)
        {
            await _table.UpsertEntityAsync<RateDo>(rate);
        }
    }

    private class RateDo : ITableEntity
    {
        public string PartitionKey { get; set; }
        
        public string RowKey { get; set; }
        
        public DateTimeOffset? Timestamp { get; set; }
        
        public ETag ETag { get; set; }

        public string RateValue { get; set; }
        
        public string TargetCurrencyCode { get; set; }
        

        public static implicit operator Rate?(RateDo? value)
        {
            if (value is null)
            {
                return null;
            }
            return new Rate(value.PartitionKey, value.TargetCurrencyCode, decimal.Parse(value.RateValue), value.Timestamp);
        }
        
        public static implicit operator RateDo(Rate value)
        {
            return new RateDo()
            {
                PartitionKey = value.BaseCurrencyCode,
                TargetCurrencyCode = value.TargetCurrencyCode,
                RateValue = value.Value.ToString(CultureInfo.InvariantCulture),
                Timestamp = value.Timestamp ?? DateTimeOffset.Now,
                RowKey = Guid.NewGuid().ToString()
            };
        }
    }
}