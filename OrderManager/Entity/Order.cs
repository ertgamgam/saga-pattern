using System.Collections.Generic;

namespace OrderManager.Entity
{
    public class Order
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Accepted;
        public IEnumerable<ItemDto> Items { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}