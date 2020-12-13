using System.Collections.Generic;
using Stock.Entity;

namespace Stock.Consumer.OrderCreated
{
    public class OrderStockUpdatedMessage
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int UserId { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}