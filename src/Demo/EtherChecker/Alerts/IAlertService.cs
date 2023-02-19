using EtherChecker.Rates;
namespace EtherChecker.Alerts;

public interface IAlertService
{
    void Alert(Rate alert);
}