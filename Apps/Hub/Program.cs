using HealthChecks.UI.Client;
using Hub.Backgrounds;
using Hub.Database;
using Hub.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Hub;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("app"));
        // builder.Services.AddSignalR();
        // bRabbitMQ consumer is not connecteduilder.Services.AddHostedService<ClientsBackgroundWorker>();

        builder.Services.AddHostedService<RabbitQueueWorker>();


        builder.Services.AddHealthChecks().AddCheck<RabbitQueueHc>(
            nameof(RabbitQueueWorker),
            Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
            ["ready", "rabbitmq"]);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        // app.MapHub<ClientsHub>("/hub/clients");
        app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            Predicate = hc => hc.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            Predicate = (_) => false
        });

        app.MapControllers();
        app.Run();
    }
}
