using Consumer.BackgroundWorkers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(
        (context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    )
    .ConfigureServices(services =>
    {
        services.AddHostedService<CheckinConsumerWorker>();
    })
    .Build();

await host.RunAsync();
