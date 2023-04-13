using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace FSH.Microservices.Infrastructure.Auth.OpenIddict;

public class OpenIddictOptions
{
    public const string SectionName = "OpenIddict";
    [Required(AllowEmptyStrings = false)]
    public string? ClientId { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? ClientSecret { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? IssuerUrl { get; set; } = string.Empty;
}

public static class OpenIddictOptionsExtensions
{
    public static OpenIddictOptions AddOpenIddictOptions(this IConfiguration configuration)
    {

        return configuration.GetSection(OpenIddictOptions.SectionName).Get<OpenIddictOptions>()!;
    }
}
