using ApiGenerator.Source.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<WeatherService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
