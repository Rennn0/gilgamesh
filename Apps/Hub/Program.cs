using Hub.Backgrounds;
using Hub.Database;
using Hub.Hubs;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true);
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
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ClientsHub>("/hub/clients");
app.MapControllers();
app.Run();
