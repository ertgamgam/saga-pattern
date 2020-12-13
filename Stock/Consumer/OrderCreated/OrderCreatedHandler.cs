using System;
using System.Threading.Tasks;
using KafkaBroker;

namespace Stock.Consumer.OrderCreated
{
    public class OrderCreatedHandler : IHandler<OrderCreatedMessage>
    {
        public async Task Handle(OrderCreatedMessage message)
        {
            Console.WriteLine(message);
        }
    }
}