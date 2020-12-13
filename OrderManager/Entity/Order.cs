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
        public IEnumerable<ItemDto> Items { get; set; }
    }

    public class ItemDto
    {
        [JsonIgnore] public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}