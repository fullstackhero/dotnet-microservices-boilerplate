using Ardalis.GuardClauses;
using AutoMapper;
using Catalog.Application.Data;
using Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Mediator;
using FSH.Infrastructure.Pagination;
using MongoDB.Driver;
using Persistence.MongoDb;

namespace Catalog.Application.Products;

public class GetProducts
{
    //Request
    public record Request(int PageNumber, int PageSize) : ICommand<PagedList<ProductDto>>;

    //Handler
    public class Handler : ICommandHandler<Request, PagedList<ProductDto>>
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;

        public Handler(CatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var products = await _context.Products.AsQueryable()
                .ApplyPagingAsync<Product, ProductDto>(_mapper.ConfigurationProvider, request.PageNumber, request.PageSize, cancellationToken);
            return products;
        }
    }
}
