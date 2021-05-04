using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokensController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = nameof(GetToken))]
        public ActionResult<string> GetToken()
        {
            var config = _configuration.GetSection(Constants.JwtKey).Get<JwtConfig>();

            string token = Auth.GenerateJwtToken(config, "test@email.com");

            return Ok(token);
        }
    }
}
