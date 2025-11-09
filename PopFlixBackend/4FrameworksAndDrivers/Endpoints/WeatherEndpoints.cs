using System.Runtime.CompilerServices;

namespace PopFlixBackend._4FrameworksAndDrivers.Endpoints
{
    public static class WeatherEndpoints 
    {
        public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder b)
        {
            string[] summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
            
            b.MapGet("weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .RequireAuthorization();

            b.MapGet("weatherforecast/{index}", (int index) =>
            {
                var forecast = new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[index]
                };

                return Results.Ok(forecast);
            });

            return b;
        }
    }
}
