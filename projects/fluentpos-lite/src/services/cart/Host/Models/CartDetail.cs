using FSH.Core.Domain;

namespace FluentPOS.Lite.Cart.Host.Models
{
    public class CartDetail : AuditableEntity
    {
        public Guid CartId { get; set; }
        public IList<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
