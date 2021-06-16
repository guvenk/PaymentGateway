using DataAccess;
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
        private readonly AppDbContext _appDbContext;

        public AuthController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        [HttpGet(Name = nameof(GetToken))]
        public ActionResult<string> GetToken()
        {
            var config = _configuration.GetSection(Constants.JwtKey).Get<JwtConfig>();

            string token = Auth.GenerateJwtToken(config, Constants.TestUser);

            return Ok(token);
        }
    }
}
