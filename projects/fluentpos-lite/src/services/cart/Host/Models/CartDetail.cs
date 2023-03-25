namespace FluentPOS.Lite.Cart.Host.Models
{
    public class CartDetail
    {
        public Guid CartId { get; set; }
        public IList<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
