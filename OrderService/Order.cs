using System;

namespace OrderService
{
    public class Order
    {
        public string Id { get; } = Guid.NewGuid().ToString();
    }
}