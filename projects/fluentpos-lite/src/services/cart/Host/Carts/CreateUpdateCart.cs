using Ardalis.GuardClauses;
using AutoMapper;
using FluentPOS.Lite.Cart.Data;
using FluentPOS.Lite.Cart.Host.Models;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace FluentPOS.Lite.Cart.Host.Carts;

public static class CreateUpdateCart
{
    //Request
    public record Request(Guid CustomerId, IList<CartItem>? CartItems) : ICommand<IResult>;

    //Validator
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(p => p.CustomerId).NotEmpty();
            RuleFor(p => p.CartItems).NotEmpty();
        }
    }

    //Handler
    public class Handler : ICommandHandler<Request, IResult>
    {
        private readonly CartDbContext _context;
        private readonly IAuthenticatedUser _user;

        public Handler(CartDbContext context, IAuthenticatedUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<IResult> Handle(Request req, CancellationToken cancellationToken)
        {
            Guard.Against.Null(req, nameof(req));
            var cart = await _context.CartDetails.Find(c => c.CartId == req.CustomerId).FirstOrDefaultAsync(cancellationToken);
            if (cart is null)
            {
                // Create a cart with details
                var newCart = new CartDetail() { CartId = req.CustomerId, CartItems = new List<CartItem>() };
                foreach (CartItem item in req.CartItems!)
                {
                    newCart.CartItems.Add(item);
                }
                newCart.CreatedBy = _user.Id;
                await _context.CartDetails.InsertOneAsync(newCart);
                return Results.Created("cart", "added new cart");
            }
            else
            {
                foreach (CartItem item in req.CartItems!)
                {
                    var existingItem = cart.CartItems!.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        cart.CartItems!.Add(item);
                    }
                }
                cart.LastModifiedBy = _user.Id;
                cart.LastModifiedOn = DateTime.UtcNow;
                _ = await _context.CartDetails.ReplaceOneAsync(x => x.CartId == req.CustomerId, cart);
                return Results.Created("cart", "updated existing cart");
            }
        }
    }
}
