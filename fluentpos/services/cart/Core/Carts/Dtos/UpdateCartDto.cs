namespace FluentPos.Cart.Core.Carts.Dtos;
public class UpdateCartDto
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
