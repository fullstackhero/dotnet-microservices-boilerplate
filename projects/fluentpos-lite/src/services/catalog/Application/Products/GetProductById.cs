using Ardalis.GuardClauses;
using AutoMapper;
using FluentPOS.Lite.Catalog.Application.Data;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Mediator;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FluentPOS.Lite.Catalog.Application.Products;

public static class GetProductById
{
    //Request
    public record Request(Guid Id) : ICommand<ProductDto>;

    //Handler
    public class Handler : ICommandHandler<Request, ProductDto>
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public Handler(CatalogDbContext context, IMapper mapper, ICacheService cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ProductDto> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var cacheKey = Product.GetCacheKey(request.Id);
            var productDto = await _cache.GetAsync<ProductDto>(cacheKey, cancellationToken);
            if (productDto is null)
            {
                var product = await _context.Products.Find(doc => doc.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
                if (product is null) throw new ProductNotFoundException(request.Id);
                productDto = _mapper.Map<ProductDto>(product);
                await _cache.SetAsync(cacheKey, productDto, cancellationToken: cancellationToken);
            }
            return productDto;
        }
    }
}
