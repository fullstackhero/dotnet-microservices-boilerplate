using FSH.Framework.Infrastructure.Options;
using System.ComponentModel.DataAnnotations;

namespace FSH.Framework.Infrastructure.Auth.OpenId;
public class OpenIdOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string? Authority { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? Audience { get; set; } = string.Empty;
}
