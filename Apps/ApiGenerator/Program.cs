using ApiGenerator.Source.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<WeatherService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
