using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FSH.Infrastructure.Common;

public class ProblemDetailsWithCode : ProblemDetails
{
    [JsonPropertyName("code")]
    public int? Code { get; set; }
}
