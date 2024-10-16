using WeatherAPI.Models;

namespace WeatherAPI.Services.Interfaces
{
    public interface IDataGenerationService
    {
        Task<IEnumerable<WeatherForecast>> GetForecast(int start, int daysToForecast);
    }
}
