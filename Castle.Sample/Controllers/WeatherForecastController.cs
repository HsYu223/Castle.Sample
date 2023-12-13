using Castle.Sample.Attributes;
using Castle.Sample.Services;
using Microsoft.AspNetCore.Mvc;

namespace Castle.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISampleService _sampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sampleService">The sample service.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISampleService sampleService)
        {
            _logger = logger;
            this._sampleService = sampleService;
        }

        [ControllerLogging("Get Weather Forecast")]
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get([FromQuery] string a)
        {
            var data = await this._sampleService.GetAsync(a);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}