using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ProjectSendGrid.Client;

public sealed class ProjectSendGridOptions
{
    public string BaseUrl { get; set; } = "https://api.your-domain.com/";
    public string ApiKey { get; set; } = string.Empty;
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectSendGrid(
        this IServiceCollection services,
        Action<ProjectSendGridOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<ProjectSendGridClient>((serviceProvider, client) =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<ProjectSendGridOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", options.ApiKey);
        });

        return services;
    }
}
