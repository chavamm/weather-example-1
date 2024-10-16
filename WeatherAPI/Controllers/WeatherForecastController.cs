using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDataGenerationService _dataGenerationService;
                
        public WeatherForecastController(IDataGenerationService dataGenerationService)
        {
            _dataGenerationService = dataGenerationService;
        }

        [Authorize]
        [HttpGet("{daysToForecast:int}", Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(int daysToForecast)
        {
            ResponseModel<IEnumerable<WeatherForecast>> response = new ResponseModel<IEnumerable<WeatherForecast>>();

            if (daysToForecast <= 0)
            {
                response.Status = false;
                response.Message = "Failed to request Get WeatherForecast";
                response.Errors.Add("El parametro `daysToForecast` debe de ser un entero mayor a cero.");

                return BadRequest(response);
            }

            var result = await _dataGenerationService.GetForecast(1, daysToForecast);

            response.Status = true;
            response.Message = "Successfully requested";
            response.Data = result;

            return Ok(response);
        }
    }
}
