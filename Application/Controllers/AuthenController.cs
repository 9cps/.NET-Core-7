using Microsoft.AspNetCore.Mvc;
using MasterService.Dto.Request;
using Application.Model;
using MasterService;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenController : ControllerBase
    {
        private readonly ILogger<AuthenController> _logger;
        private readonly IMasterService _masterService; // Change the type to IMasterService


        public AuthenController(ILogger<AuthenController> logger, IMasterService masterService)
        {
            _logger = logger;
            _masterService = masterService;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public ResponseModel RegisterUser(InsertUser req)
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

        [HttpGet("LoginByGoogle")]
        public IActionResult LoginByGoogle()
        {
            // Set the URL the user will be redirected to after signing in with Google.
            var redirectUrl = Url.Action("GoogleCallback", "Authen", null, Request.Scheme);

            // Request a redirect to the Google authentication endpoint.
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback()
        {
            // Handle the callback after the user signs in with Google.
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                // Handle authentication failure.
                return Unauthorized();
            }

            // Authentication succeeded.

            // Access the basic user information from the authentication result.
            var userEmail = result.Principal.FindFirstValue(ClaimTypes.Email);

            // Fetch more user information from the Google API using the access token.
            var accessToken = result.Properties.GetTokenValue("access_token");
            var userInfo = await GetGoogleUserInfo(accessToken);

            // Log the response content to the console.
            var responseContent = userInfo.ToString();
            Console.WriteLine(responseContent);

            // Continue with your logic for handling user information.

            // Redirect or return a success response.
            return Ok("Google login successful!");
        }

        private async Task<JObject> GetGoogleUserInfo(string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseContent);
        }
    }
}