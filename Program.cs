var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add app insights telemetry service
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = GenerateWeatherForecast();
    // log warning with date and time
    app.Logger.LogWarning("Weather forecast requested at {date}", DateTime.Now);
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// create a new operation that returns the weather forecast ordered by temperatureC
app.MapGet("/weatherforecast/ordered", () =>
{
    var forecast = GenerateWeatherForecast().OrderBy(f => f.TemperatureC).ToArray();
    // log warning
    app.Logger.LogWarning("Weather forecast ordered by temperatureC requested at {date}", DateTime.Now);
    return forecast;
}).WithName("GetWeatherForecastOrdered")
.WithOpenApi();

IEnumerable<WeatherForecast> GenerateWeatherForecast()
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
