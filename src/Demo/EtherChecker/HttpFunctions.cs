using System.Text.Json;
using EtherChecker.Rates;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EtherChecker;

public class HttpFunctions
{
    private readonly IRateStorage _storage;
    private readonly ILogger<HttpFunctions> _logger;

    public HttpFunctions(ILogger<HttpFunctions> logger, IRateStorage storage)
    {
        _storage = storage;
        _logger = logger;
    }
    
    [Function(nameof(Rate))]
    public async Task<HttpResponseData> Rate([HttpTrigger("GET")] HttpRequestData request)
    {
        var response = request.CreateResponse();
        var parameters =  System.Web.HttpUtility.ParseQueryString(request.Url.Query);
        var currency = parameters["currency"];
        
        if (string.IsNullOrEmpty(currency))
        {
            _logger.LogDebug("Currency not specified, assuming {Default}", Currencies.DefaultCurrency);
            currency = Currencies.DefaultCurrency;
        }
        
        var rate = _storage.GetRate(currency);

        await response.WriteStringAsync(rate is null
            ? $"Rate for currency {currency} was not found"
            : JsonSerializer.Serialize(rate));

        return response;
    }
}