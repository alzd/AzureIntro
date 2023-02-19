namespace EtherChecker.Rates;

public interface IEtherRateService
{
    Task<List<Rate>> GetRatesAsync();
    
    bool TriggerAlert(IList<Rate> rates, out Rate? alert);
}