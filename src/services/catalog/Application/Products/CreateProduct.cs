using Ardalis.GuardClauses;
using AutoMapper;
using Catalog.Application.Data;
using Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Dto;
using FSH.Core.Mediator;

namespace Catalog.Application.Products;

public static class CreateProduct
{
    //Request
    public record Request(string Name, int Quantity, decimal Price) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

    //Validator
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(75);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(1);
        }
    }

    //Handler
    public class Handler : ICommandHandler<Request, Response>
    {
        private readonly CatalogDbContext _context;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;

        public Handler(CatalogDbContext context, ICacheService cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var product = Product.Create(request.Name, request.Quantity, request.Price);
            await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
            var productDto = _mapper.Map<ProductDto>(product);
            await _cache.SetAsync(Product.GenerateCacheKey(product.Id), productDto, cancellationToken: cancellationToken);
            return new Response(product.Id);
        }
    }
}
