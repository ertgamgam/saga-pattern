using System.Collections.Generic;

namespace Wallet.Consumer.OrderStockUpdated
{
    public class OrderStockUpdatedMessage
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ProductPriceDto> Products { get; set; }
    }

    public class ProductPriceDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}