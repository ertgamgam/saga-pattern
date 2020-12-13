using System.Collections.Generic;

namespace Stock.Consumer.OrderCreated
{
    public class OrderCreatedMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ItemDto> Items { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}