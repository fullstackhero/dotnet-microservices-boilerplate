using FSH.Framework.Infrastructure.Options;

namespace FSH.Framework.Infrastructure.Logging.Serilog;

public class SerilogOptions : IOptionsRoot
{
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public int RetentionFileCount { get; set; } = 5;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
    public bool EnableErichers { get; set; } = true;
    public bool OverideMinimumLogLevel { get; set; } = true;
}