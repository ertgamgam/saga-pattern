using System.Collections.Generic;

namespace Wallet.Consumer.OrderStockUpdated
{
    public class OrderWalletPayErrorMessage
    {
        public int OrderId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}