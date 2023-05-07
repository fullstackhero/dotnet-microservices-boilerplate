using FSH.Framework.Core.Domain;

namespace FluentPos.Cart.Core.Carts;

public class CustomerCart : BaseEntity
{
    public Guid CustomerId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public CustomerCart(Guid customerId)
    {
        CustomerId = customerId;
    }

    public CustomerCart AddItem(Guid productId, int quantity)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }
        this.UpdateModifiedProperties(DateTime.UtcNow, null!);
        return this;
    }
}
