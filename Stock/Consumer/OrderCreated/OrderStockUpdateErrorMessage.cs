namespace Stock.Consumer.OrderCreated
{
    public class OrderStockUpdateErrorMessage
    {
        public int OrderId { get; set; }
        public StockUpdateError StockUpdateError { get; set; }
    }
}