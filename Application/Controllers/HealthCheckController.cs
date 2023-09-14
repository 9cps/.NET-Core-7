using Microsoft.AspNetCore.Mvc;
using MasterService.Dto.Request;
using Application.Model;
using MasterService;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMasterService _masterService; // Change the type to IMasterService


        public HealthCheckController(ILogger<WeatherForecastController> logger, IMasterService masterService)
        {
            _logger = logger;
            _masterService = masterService;
        }

        [HttpGet]
        [Route("HealthCheckApi")]
        public ResponseModel HealthCheckApi()
        {
            var result = new ResponseModel();
            try
            {
                result.Data = "Ok";
                result.Message = "The api works normally.";
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("HealthCheckDataBase")]
        public ResponseModel HealthCheckDataBase()
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.HealthCheckConnection();
                result.Message = "The connection works normally.";
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