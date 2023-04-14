using FSH.Microservices.Infrastructure.Options;
using System.ComponentModel.DataAnnotations;

namespace FSH.Microservices.Infrastructure.Logging.Serilog;

public class SerilogOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string AppName { get; set; } = "FSH.WebAPI";
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
    public bool MinimalLogging { get; set; } = false;
}