using Azure.Data.Tables;
using EtherChecker.Alerts;
using EtherChecker.Rates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(config =>
    {
        config.AddEnvironmentVariables()
            .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((builder, services) =>
    {
        services.AddTransient<IEtherRateService, EtherRateService>();
        services.AddHttpClient<IEtherRateService, EtherRateService>();
        services.AddSingleton<IRateStorage, RateStorage>();
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton(new TableServiceClient(builder.Configuration["AzureWebJobsStorage"]));
    })
    .Build();

host.Run();