using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace FluentPOS.Auth.Host.Handlers;

public class TokenResponseHandler : IOpenIddictServerHandler<ApplyTokenResponseContext>
{
    private readonly ILogger<TokenResponseHandler> _logger;

    public TokenResponseHandler(ILogger<TokenResponseHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask HandleAsync(ApplyTokenResponseContext context)
    {
        var response = context.Response;
        var transaction = context.Transaction;
        if (string.Equals(response.Error, OpenIddictConstants.Errors.InvalidClient, StringComparison.Ordinal) &&
           !string.IsNullOrEmpty(response.ErrorDescription))
        {
            _logger.LogError(response.Error);
        }

        return default;
    }
}
