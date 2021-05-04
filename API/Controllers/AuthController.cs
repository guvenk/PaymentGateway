using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("token", Name = nameof(GetToken))]
        public ActionResult<string> GetToken()
        {
            var config = _configuration.GetSection(Constants.JwtKey).Get<JwtConfig>();

            string token = AuthUtil.GenerateJwtToken(config, "test@email.com");

            return Ok(token);
        }
    }
}
