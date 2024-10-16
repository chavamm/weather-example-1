using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
            {

                response.Status = false;
                response.Message = "Unauthorized";
                response.Errors.Add("Usuario o contraseña no validos.");

                return Unauthorized(response);
            }

            response.Status = true;
            response.Message = "Successfully";
            response.Data = token;

            return Ok(response);
        }



    }
}
