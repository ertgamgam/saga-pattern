using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Entity;
using Stock.Repository;

namespace Stock.Consumer.OrderCreated
{
    public class OrderCreatedHandler : IHandler<OrderCreatedMessage>
    {
        private const string OrderStockUpdatedTopicName = "order-stock-updated";
        private const string OrderStockUpdateErrorTopic = "order-stock-update-error";

        private readonly StockDbContext _dbContext;
        private readonly ILogger<OrderCreatedHandler> _logger;
        private readonly IKafkaMessageProducer _kafkaMessageProducer;

        public OrderCreatedHandler(StockDbContext dbContext, ILogger<OrderCreatedHandler> logger,
            IKafkaMessageProducer kafkaMessageProducer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _kafkaMessageProducer = kafkaMessageProducer;
        }

        public async Task Handle(OrderCreatedMessage message)
        {
            _logger.LogInformation($"Order created message received. Order Id = {message.Id}");
            var messageProducts = message.Products.ToList();
            var productIds = messageProducts.Select(x => x.ProductId).ToList();
            var dbProducts = await _dbContext.Products
                .Where(product => productIds.Contains(product.Id))
                .ToListAsync();

            if (IsThereEnoughStock(messageProducts, dbProducts))
            {
                await DecreaseStock(message, dbProducts);
                await ProduceOrderStockUpdatedMessage(message, dbProducts);
            }
            else
            {
                await ProduceOrderStockUpdateErrorMessage(message);
            }

            Console.WriteLine(message);
        }

        private async Task DecreaseStock(OrderCreatedMessage message, List<Product> dbProducts)
        {
            dbProducts.ForEach(dbProduct =>
            {
                var quantity = message.Products
                    .FirstOrDefault(messageProduct => messageProduct.ProductId == dbProduct.Id)?
                    .Quantity ?? default;
                dbProduct.StockQuantity -= quantity;
            });
            await _dbContext.SaveChangesAsync();
        }

        private async Task ProduceOrderStockUpdateErrorMessage(OrderCreatedMessage orderCreatedMessage)
        {
            await _kafkaMessageProducer.Produce(OrderStockUpdatedTopicName, orderCreatedMessage.Id.ToString(),
                new OrderStockUpdateErrorMessage
                {
                    OrderId = orderCreatedMessage.Id,
                    StockUpdateError = StockUpdateError.StockNotEnough
                });
        }

        private async Task ProduceOrderStockUpdatedMessage(OrderCreatedMessage message, List<Product> dbProducts)
        {
            var orderStockUpdatedMessage = new OrderStockUpdatedMessage
            {
                OrderId = message.Id,
                OrderName = message.Name,
                UserId = message.UserId,
                Products = dbProducts
            };
            await _kafkaMessageProducer.Produce(OrderStockUpdatedTopicName, orderStockUpdatedMessage.OrderId.ToString(),
                orderStockUpdatedMessage);
        }

        private bool IsThereEnoughStock(List<ProductDto> messageProducts, List<Product> dbProducts)
        {
            foreach (var messageProduct in messageProducts)
            {
                var product = dbProducts.FirstOrDefault(x => x.Id == messageProduct.ProductId);
                if (product?.StockQuantity < messageProduct.Quantity)
                {
                    return false;
                }
            }

            return true;
        }
    }
}