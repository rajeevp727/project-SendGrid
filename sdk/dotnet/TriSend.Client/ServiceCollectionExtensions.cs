using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TriSend.Client;

public sealed class TriSendOptions
{
    public string BaseUrl { get; set; } = "https://api.your-domain.com/";
    public string ApiKey { get; set; } = string.Empty;
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriSend(
        this IServiceCollection services,
        Action<TriSendOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<TriSendClient>((serviceProvider, client) =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<TriSendOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", options.ApiKey);
        });

        return services;
    }
}
