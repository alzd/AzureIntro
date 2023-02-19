namespace EtherChecker.Rates;

public interface IRateStorage
{
    Rate? GetRate(string currency);
    
    Task SaveRatesAsync(IList<Rate> rates);
}