using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Core.Products.Features;
public static class DeleteProduct
{
    public sealed record Command : IRequest
    {
        public readonly Guid Id;
        public Command(Guid id)
        {
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByIdAsync(request.Id, cancellationToken);
        }
    }
}
