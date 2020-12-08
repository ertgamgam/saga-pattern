using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;
using OrderManager.Repository;

namespace OrderManager.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrderDbContext _dbContext;

        public OrderController(ILogger<OrderController> logger, OrderDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var entityEntry = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            Response.Headers.Add("Location", $"/orders/{entityEntry.Entity.Id}");
            return Accepted();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            return result is null ? NotFound() : Ok(result) as IActionResult;
        }
    }
}