using System.Threading.Tasks;
using KafkaBroker;
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
        private const string OrderCreatedTopicName = "order-created";

        private readonly ILogger<OrderController> _logger;
        private readonly OrderDbContext _dbContext;
        private readonly IKafkaMessageProducer _messageProducer;

        public OrderController(ILogger<OrderController> logger, OrderDbContext dbContext,
            IKafkaMessageProducer messageProducer)
        {
            _logger = logger;
            _dbContext = dbContext;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var entityEntry = await _dbContext.Orders.AddAsync(order);
            var savedOrder = entityEntry.Entity;
            _logger.LogInformation($"Order accepted. Order Id = {savedOrder.Id}");

            await _dbContext.SaveChangesAsync();
            await _messageProducer.Produce(OrderCreatedTopicName, savedOrder.Id.ToString(), savedOrder);
            _logger.LogInformation($"OrderCreated message was sent. Order Id = {savedOrder.Id}");

            Response.Headers.Add("Location", $"/orders/{savedOrder.Id}");
            return Accepted();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Orders.Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
            return result is null ? NotFound() : Ok(result) as IActionResult;
        }
    }
}