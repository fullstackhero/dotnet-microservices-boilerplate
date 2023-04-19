using FSH.Microservices.Infrastructure.Options;

namespace FSH.Microservices.Infrastructure.Swagger
{
    public class SwaggerOptions : IOptionsRoot
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
