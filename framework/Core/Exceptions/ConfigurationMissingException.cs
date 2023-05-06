using System.Net;

namespace FSH.Framework.Core.Exceptions;

public class ConfigurationMissingException : CustomException
{
    public ConfigurationMissingException(string sectionName) : base($"{sectionName} Missing in Configurations", HttpStatusCode.NotFound)
    {
    }

    public ConfigurationMissingException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(message, statusCode)
    {
    }
}