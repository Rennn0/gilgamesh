namespace ApiGenerator.Source.Services;

public class WeatherService
{
    public Task<GetWeatherResponse> GetWeatherAsync() =>
        Task.FromResult<GetWeatherResponse>(
            new GetWeatherResponse()
            {
                Forecast = new List<string> { "Sunny", "Cloudy", "Rainy", "Snowy" },
            }
        );
}

public class GetWeatherRequest
{
    public string Location { get; set; }
}

public class GetWeatherResponse
{
    public IEnumerable<string> Forecast { get; set; }
}
