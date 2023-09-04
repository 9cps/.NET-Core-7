using Microsoft.AspNetCore.Mvc;
using MasterService.Dto.Request;
using Application.Model;
using MasterService;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelpCheckController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMasterService _masterService; // Change the type to IMasterService


        public HelpCheckController(ILogger<WeatherForecastController> logger, IMasterService masterService)
        {
            _logger = logger;
            _masterService = masterService;
        }

        [HttpGet]
        [Route("HelpCheckApi")]
        public ResponseModel HelpCheckApi()
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
        [Route("HelpCheckDataBase")]
        public ResponseModel HelpCheckDataBase()
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.HelpCheckConnection();
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