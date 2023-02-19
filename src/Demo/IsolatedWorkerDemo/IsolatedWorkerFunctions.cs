using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IsolatedWorkerDemo;

public class IsolatedWorkerFunctions
{
    [Function(nameof(HelloTimer))]
    public void HelloTimer([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, FunctionContext context)
    {
        var logger = context.GetLogger("HelloTimer");
        logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
    }
}
