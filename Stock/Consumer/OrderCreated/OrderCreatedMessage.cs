using System.Collections.Generic;

namespace Stock.Consumer.OrderCreated
{
    public class OrderCreatedMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}