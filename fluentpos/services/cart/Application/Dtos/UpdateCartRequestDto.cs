using FluentPos.Cart.Domain;

namespace FluentPos.Cart.Application.Dtos;
public class UpdateCartRequestDto
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
