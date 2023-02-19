#nullable enable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DefaultWorkerDemo;

public static class InProcessFunctions
{
    [FunctionName(nameof(Hello))]
    public static async Task<IActionResult> Hello(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log,
        [Queue("hello", Connection = "AzureWebJobsStorage")] IAsyncCollector<string?> queueMessage
        )
    {
        log.LogInformation("C# HTTP trigger function processed a request");

        var name = (string?)req.Query["name"];

        if (string.IsNullOrEmpty(name))
        {
            return new BadRequestObjectResult("Please pass a name in the query string");
        }
        
        await queueMessage.AddAsync(name);
        return new OkObjectResult($"Hello, {name}");
    }

    [FunctionName(nameof(HelloQueue))]
    public static void HelloQueue(
        [QueueTrigger("hello")] string name,
        ILogger log)
    {
        log.LogInformation("C# Queue trigger function processed: {Name}", name);

    }
    
}