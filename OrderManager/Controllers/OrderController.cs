using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManager.Entity;
using OrderManager.EventPublisher;
using OrderManager.Repository;

namespace OrderManager.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrderDbContext _dbContext;
        private readonly IOrderEventPublisher _orderEventPublisher;

        public OrderController(ILogger<OrderController> logger, OrderDbContext dbContext,
            IOrderEventPublisher orderEventPublisher)
        {
            _logger = logger;
            _dbContext = dbContext;
            _orderEventPublisher = orderEventPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var entityEntry = await _dbContext.Orders.AddAsync(order);
            var savedOrder = entityEntry.Entity;

            await _orderEventPublisher.PublishOrderCreateEvent(savedOrder);
            await _dbContext.SaveChangesAsync();

            Response.Headers.Add("Location", $"/orders/{savedOrder.Id}");
            return Accepted();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Orders.Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            return result is null ? NotFound() : Ok(result) as IActionResult;
        }
    }
}