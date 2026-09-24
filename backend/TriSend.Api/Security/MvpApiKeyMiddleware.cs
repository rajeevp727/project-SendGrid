namespace TriSend.Api.Security;

public sealed class MvpApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _apiKey = configuration["Mvp:ApiKey"] ?? string.Empty;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await next(context);
            return;
        }

        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(new
            {
                code = "api_not_configured",
                message = "MVP API key is not configured."
            });
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var authorization) ||
            authorization.Count != 1 ||
            !authorization[0].StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(authorization[0]["Bearer ".Length..], _apiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                code = "unauthorized",
                message = "A valid tenant API key is required."
            });
            return;
        }

        await next(context);
    }
}
