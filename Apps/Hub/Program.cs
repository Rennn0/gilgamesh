using Hub.Backgrounds;
using Hub.Database;
using Hub.Hubs;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                }
            );
        });


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("app"));
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<ClientsBackgroundWorker>();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.MapHub<ClientsHub>("/hub/clients");
        app.MapControllers();
        app.Run();
    }
}