using System.Collections.Generic;

namespace Stock.Consumer.OrderWalletPayError
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