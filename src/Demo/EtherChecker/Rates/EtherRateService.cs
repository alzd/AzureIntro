using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace EtherChecker.Rates;

class EtherRateService : IEtherRateService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public EtherRateService(HttpClient httpClient, IConfiguration config)
    {
        _config = config;
        _httpClient = httpClient;
    }

    private decimal? _threshold = null;
    public decimal Threshold {
        get
        {
            if (_threshold is null)
            {
                if (decimal.TryParse(_config["AlertThreshold"], out var threshold))
                {
                    _threshold = threshold;
                }
                else
                {
                    _threshold = -1;
                }
            }

            return _threshold.Value;
        }
    }

    private string ApiKey => _config["CoinApiKey"] ?? string.Empty;
    
    public async Task<List<Rate>> GetRatesAsync()
    {
        var url = new Uri("https://rest.coinapi.io/v1/exchangerate/ETH?invert=false");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-CoinAPI-Key", ApiKey);

        var response = await _httpClient.SendAsync(request);
        var rates = await response.Content.ReadFromJsonAsync<RateResponseDto>();
        return rates;
    }

    public bool TriggerAlert(IList<Rate> rates, out Rate? alert)
    {
        if (Threshold <= 0)
        {
            alert = default;
            return false;
        }

        alert = rates
            .Where(rate =>
                rate.TargetCurrencyCode.Equals(Currencies.DefaultCurrency, StringComparison.InvariantCultureIgnoreCase))
            .Where(rate => rate.Value >= Threshold)
            .MaxBy(rate => rate.Timestamp);

        return alert is not null;
    }
}