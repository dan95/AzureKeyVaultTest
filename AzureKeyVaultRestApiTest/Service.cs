using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureKeyVaultRestApiTest
{
    public class Service : IHostedService
    {
        private readonly AzureApiSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<Service> _logger;

        public Service(
            IOptions<AzureApiSettings> options,
            HttpClient httpClient,
            ILogger<Service> logger,
            IConfiguration configuration
            )
        {
            _settings = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Class} | {Method} | Application starting", nameof(Service), nameof(StartAsync));

            var authRequest = new HttpRequestMessage(
                HttpMethod.Post,
                requestUri: string.Format(_settings.AzureApiAuthorizationSettings.AuthorizationUri, _settings.AzureApiKeyVaultSettings.TenantId)
            );
            authRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", _settings.AzureApiAuthorizationSettings.GrantType },
                { "client_secret", _settings.AzureApiKeyVaultSettings.ClientSecret },
                { "client_id", _settings.AzureApiKeyVaultSettings.ClientId },
                { "scope", _settings.AzureApiKeyVaultSettings.AuthorizationScope }
            });


            _logger.LogInformation("{Class} | {Method} | Requesting Azure authorization token", nameof(Service), nameof(StartAsync));
            var authResponse = await _httpClient.SendAsync(authRequest, HttpCompletionOption.ResponseContentRead, cancellationToken);

            authResponse.EnsureSuccessStatusCode();

            var authResponseResult = await authResponse.Content.ReadAsStringAsync(cancellationToken);

            var loginToken = JsonSerializer.Deserialize<LoginToken>(authResponseResult);

            _logger.LogInformation("{Class} | {Method} | Successfully logged in. Token '{AccessToken}...'",
                nameof(Service),
                nameof(StartAsync),
                loginToken!.AccessToken.Substring(0, 20));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri: $"{_settings.AzureApiKeyVaultSettings.KeyVaultUri}/secrets?api-version=7.4");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginToken!.AccessToken);

            _logger.LogInformation("{Class} | {Method} | Querying secrets",
                nameof(Service),
                nameof(StartAsync));

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var secretResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("{Class} | {Method} | Secrets succesfully retrieved | {SecretResult}",
                nameof(Service),
                nameof(StartAsync),
                secretResponse);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("{Class} | {Method} | Application stopping", nameof(Service), nameof(StartAsync));
            await Task.CompletedTask;
        }
    }
}
