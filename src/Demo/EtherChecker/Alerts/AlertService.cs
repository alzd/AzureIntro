using System.Text.Json;
using EtherChecker.Rates;
using Microsoft.Extensions.Logging;

namespace EtherChecker.Alerts;

public class AlertService : IAlertService
{
    private readonly ILogger<AlertService> _logger;

    public AlertService(ILogger<AlertService> logger)
    {
        _logger = logger;
    }
    public void Alert(Rate alert)
    {
        _logger.LogWarning("Alert for rate: {Rate}", JsonSerializer.Serialize(alert));
    }
}