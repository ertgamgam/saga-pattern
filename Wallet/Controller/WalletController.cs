using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Repository;

namespace Wallet.Controller
{
    [ApiController]
    [Route("wallets")]
    public class WalletController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;

        public WalletController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var wallets = await _dbContext.Products.ToListAsync();
            return Ok(new {Wallets = wallets});
        }
    }
}