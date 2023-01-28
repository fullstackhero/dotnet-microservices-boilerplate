using Ardalis.GuardClauses;
using AutoMapper;
using Catalog.Application.Data;
using Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace Catalog.Application.Products;

public static class GetProductById
{
    //Request
    public record Request(Guid Id) : ICommand<ProductDto>;

    //Handler
    public class Handler : ICommandHandler<Request, ProductDto>
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;

        public Handler(CatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var product = await _context.Products.Find(doc => doc.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (product is null) throw new ProductNotFoundException(request.Id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
