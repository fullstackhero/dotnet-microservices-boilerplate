using Ardalis.GuardClauses;
using FluentPOS.Lite.Catalog.Application.Data;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Dto;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace FluentPOS.Lite.Catalog.Application.Products;

public static class DeleteProduct
{
    //Request
    public record Request(Guid Id) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

    //Handler
    public class Handler : ICommandHandler<Request, Response>
    {
        private readonly CatalogDbContext _context;
        private readonly ICacheService _cache;

        public Handler(CatalogDbContext context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var result = await _context.Products.DeleteOneAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (result.DeletedCount == 0) throw new ProductNotFoundException(request.Id);
            await _cache.RemoveAsync(Product.GetCacheKey(request.Id), cancellationToken);
            return new Response(request.Id);
        }
    }
}
