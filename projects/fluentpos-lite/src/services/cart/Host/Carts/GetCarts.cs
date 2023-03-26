using Ardalis.GuardClauses;
using FluentPOS.Lite.Cart.Data;
using FluentPOS.Lite.Cart.Host.Models;
using FluentValidation;
using FSH.Core.Mediator;
using FSH.Infrastructure.Pagination;
using FSH.Persistence.MongoDb;
using MongoDB.Driver;

namespace FluentPOS.Lite.Cart.Host.Carts
{
    public static class GetCarts
    {
        //Request
        public record Request(int PageNumber, int PageSize) : ICommand<PagedList<CartDetail>>;

        //Handler
        public class Handler : ICommandHandler<Request, PagedList<CartDetail>>
        {
            private readonly CartDbContext _context;

            public Handler(CartDbContext context)
            {
                _context = context;
            }

            public async Task<PagedList<CartDetail>> Handle(Request req, CancellationToken cancellationToken)
            {
                Guard.Against.Null(req, nameof(req));
                return await _context
                                .CartDetails
                                .AsQueryable()
                                .ApplyPagingAsync(req.PageNumber, req.PageSize, cancellationToken);
            }
        }
    }
}
