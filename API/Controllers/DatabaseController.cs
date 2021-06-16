using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public DatabaseController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet(Name = nameof(UpdateDatabase))]
        public async Task<ActionResult> UpdateDatabase()
        {
            await _appDbContext.Database.MigrateAsync();

            return Ok();
        }
    }
}
