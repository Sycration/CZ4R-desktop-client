using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using App.Models;

namespace App.Services;

public class ServerUrlHandler : DelegatingHandler
{
    private readonly AppState _state;

    public ServerUrlHandler(AppState state)
    {
        _state = state;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = _state.ServerUrl?.TrimEnd('/');
        if (!string.IsNullOrEmpty(url) && request.RequestUri is { } uri && uri.IsAbsoluteUri)
        {
            var baseUrl = new Uri(url);
            request.RequestUri = new UriBuilder(uri)
            {
                Scheme = baseUrl.Scheme,
                Host = baseUrl.Host,
                Port = baseUrl.Port
            }.Uri;
        }

        if (!string.IsNullOrEmpty(_state.Token) && request.Headers.Authorization is null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _state.Token);

        return base.SendAsync(request, cancellationToken);
    }
}