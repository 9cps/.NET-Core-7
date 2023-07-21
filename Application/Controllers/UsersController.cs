using Microsoft.AspNetCore.Mvc;
using MasterService.Dto.Request;
using Application.Model;
using MasterService;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMasterService _masterService; // Change the type to IMasterService


        public UsersController(ILogger<UsersController> logger, IMasterService masterService)
        {
            _logger = logger;
            _masterService = masterService;
        }

        [HttpPost]
        [Route("GetUsersByKeyword")]
        public ResponseModel GetUsersByKeyword(GetUsersByKeyword req)
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.GetUsersByKeyword(req);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            
            return result;
        }

        [HttpPost]
        [Route("CreateUser")]
        public ResponseModel CreateUser(InsertUser req)
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.InsertUser(req);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("LoginByUser")]
        public ResponseModel LoginByUser(LoginByUser req)
        {
            var result = new ResponseModel();
            try
            {
                result.Data = _masterService.LoginByUser(req);
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