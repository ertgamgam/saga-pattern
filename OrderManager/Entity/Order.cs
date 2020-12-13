using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderManager.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Accepted;
        public string FailCause { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        [JsonIgnore] public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}