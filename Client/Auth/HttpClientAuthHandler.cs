using System.Net;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.Auth;

public class HttpClientAuthHandler(IHttpClientFactory httpClientFactory, IOptions<AuthSettings> options)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken ct)
    {
        var accessToken = await GetAccessToken(ct);
        request.SetBearerToken(accessToken);

        var res = await base.SendAsync(request, ct);

        if (res.StatusCode == HttpStatusCode.Forbidden) throw new ForbiddenException();

        return res;
    }

    private async Task<string> GetAccessToken(CancellationToken ct)
    {
        var authSettings = options.Value;
        var httpClient = httpClientFactory.CreateClient();

        var discovery = await httpClient.GetDiscoveryDocumentAsync(authSettings.Authority, ct);

        var tokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = authSettings.ClientId,
            ClientSecret = authSettings.ClientSecrets,
            Address = discovery.TokenEndpoint
        };

        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest, ct);

        return tokenResponse.AccessToken;
    }
}