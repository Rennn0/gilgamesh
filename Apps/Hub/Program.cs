using Enyim.Diagnostics;
using Hub.Database;
using Hub.Refit;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Prometheus.DotNetRuntime;
using Refit;
using Stripe;
using LogLevel = Enyim.LogLevel;

namespace Hub;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY");

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder
            .Services.AddRefitClient<IDocsApi>()
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    string url =
                        provider.GetRequiredService<IConfiguration>()["DocsUrl"]
                        ?? throw new Exception();
                    client.BaseAddress = new Uri(url);
                }
            );

        builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("app"));
        // builder.Services.AddSignalR();
        // bRabbitMQ consumer is not connecteduilder.Services.AddHostedService<ClientsBackgroundWorker>();

        // builder.Services.AddHostedService<RabbitQueueWorker>();


        // builder.Services.AddHealthChecks().AddCheck<RabbitQueueHc>(
        //     nameof(RabbitQueueWorker),
        //     Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
        //     ["ready", "rabbitmq"]);

        Enyim.LogManager.AssignFactory(new ConsoleLoggerFactory(LogLevel.Information));

        builder.Services.AddMemcached("localhost:11211");

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        // app.MapHub<ClientsHub>("/hub/clients");
        // app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        // {
        //     Predicate = hc => hc.Tags.Contains("ready"),
        //     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        // });

        // app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        // {
        //     Predicate = (_) => false
        // });

        DotNetRuntimeStatsBuilder
            .Default()
            .WithErrorHandler(ex => Console.WriteLine(ex.Message))
            .StartCollecting();

        app.UseMetricServer();
        app.UseHttpMetrics();

        app.MapControllers();
        app.Run();
    }
}
