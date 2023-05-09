namespace FluentPos.Cart.Core.Dtos;
public class UpdateCartRequestDto
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
