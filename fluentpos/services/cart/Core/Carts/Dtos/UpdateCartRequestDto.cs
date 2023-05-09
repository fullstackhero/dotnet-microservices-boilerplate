namespace FluentPos.Cart.Core.Carts.Dtos;
public class UpdateCartRequestDto
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
