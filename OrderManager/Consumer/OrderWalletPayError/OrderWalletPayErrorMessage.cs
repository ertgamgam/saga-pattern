namespace OrderManager.Consumer.OrderWalletPayError
{
    public class OrderWalletPayErrorMessage
    {
        public int OrderId { get; set; }
        public string WalletError { get; set; }
    }
}