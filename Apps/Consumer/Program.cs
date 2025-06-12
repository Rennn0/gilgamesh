using Consumer.BackgroundWorkers;
using Consumer.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, logging) =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureAppConfiguration(
        (context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    )
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMq"));

        services.AddHostedService<CheckinConsumerWorker>();
    })
    .Build();

await host.RunAsync();
