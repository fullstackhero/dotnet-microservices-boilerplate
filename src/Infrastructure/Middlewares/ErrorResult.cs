namespace FSH.Microservices.Infrastructure.Middlewares;

public class ErrorResult
{
    public List<string>? Errors { get; set; }
    public string? ErrorId { get; set; }
}
