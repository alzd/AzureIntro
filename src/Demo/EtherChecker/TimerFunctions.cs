using EtherChecker.Alerts;
using EtherChecker.Rates;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EtherChecker;

public class TimerFunctions
{
    private readonly ILogger<TimerFunctions> _logger;
    private readonly IEtherRateService _rateService;
    private readonly IRateStorage _storage;
    private readonly IAlertService _alertService;

    public TimerFunctions(ILogger<TimerFunctions> logger, IEtherRateService rateService, IRateStorage storage, IAlertService alertService)
    {
        _logger = logger;
        _rateService = rateService;
        _storage = storage;
        _alertService = alertService;
    }

    [Function(nameof(CheckRates))]
    public async Task CheckRates([TimerTrigger("0 */1 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Checking rates");
        
        var rates = await _rateService.GetRatesAsync();
        await _storage.SaveRatesAsync(rates);

        if (_rateService.TriggerAlert(rates, out var rate))
        {
            _alertService.Alert(rate);
        }
    }
}