using Microsoft.AspNetCore.Mvc;
using MasterService.Dto.Request;
using Application.Model;
using MasterService;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMasterService _masterService; // Change the type to IMasterService


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMasterService masterService)
        {
            _logger = logger;
            _masterService = masterService;
        }

        [Authorize]
        [HttpPost]
        [Route("GetWeatherForecast")]
        public ResponseModel GetWeatherForecast(GetWeatherForecast req)
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.GetWeatherForecast(req);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            
            return result;
        }
    }
}