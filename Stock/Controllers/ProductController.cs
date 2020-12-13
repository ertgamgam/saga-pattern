using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stock.Repository;

namespace Stock.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly StockDbContext _dbContext;

        public ProductController(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = _dbContext.Products.ToList();
            return Ok(new {Products = products});
        }
    }
}