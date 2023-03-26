using Ardalis.GuardClauses;
using FluentPOS.Lite.Cart.Data;
using FluentValidation;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace FluentPOS.Lite.Cart.Host.Carts;

public static class GetCartDetails
{
    //Request
    public record Request(Guid CustomerId) : ICommand<IResult>;

    //Validator
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(p => p.CustomerId).NotEmpty();
        }
    }

    //Handler
    public class Handler : ICommandHandler<Request, IResult>
    {
        private readonly CartDbContext _context;

        public Handler(CartDbContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(Request req, CancellationToken cancellationToken)
        {
            Guard.Against.Null(req, nameof(req));
            var cart = await _context.CartDetails.Find(c => c.CartId == req.CustomerId).FirstOrDefaultAsync(cancellationToken);
            if (cart is null)
            {
                return Results.NotFound(req.CustomerId);
            }
            else
            {
                return Results.Ok(cart);
            }
        }
    }
}
