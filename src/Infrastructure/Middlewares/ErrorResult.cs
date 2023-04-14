namespace FSH.Microservices.Infrastructure.Middlewares;

public class ErrorResult
{
    public List<string>? Messages { get; set; }
    public string? Source { get; set; }
    public string? Exception { get; set; }
    public string? ErrorId { get; set; }

    public string? StackTrace { get; set; }
}
