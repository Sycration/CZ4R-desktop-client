using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Models;
using Org.OpenAPITools;
using Org.OpenAPITools.Client;

namespace App.Services;

public class AuthenticationTokenProvider : TokenProvider<BearerToken>
{
    private readonly AppState _state;

    public AuthenticationTokenProvider(AppState state)
    {
        _state = state;
    }

    protected override ValueTask<BearerToken> GetAsync(string header = "", CancellationToken cancellation = default)
    {
        var token = _state.Token;
        return new ValueTask<BearerToken>(new OptionalBearerToken(token));
    }
}

public class OptionalBearerToken : BearerToken
{
    private readonly bool _hasToken;

    public OptionalBearerToken(string? token, TimeSpan? timeout = null)
        : base(token ?? string.Empty, timeout)
    {
        _hasToken = !string.IsNullOrEmpty(token);
    }

    public override void UseInHeader(HttpRequestMessage request, string headerName)
    {
        if (_hasToken)
            base.UseInHeader(request, headerName);
    }
}