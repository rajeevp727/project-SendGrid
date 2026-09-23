using Communication.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();


app.UseMiddleware<MvpApiKeyMiddleware>();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "communication-api",
    utc = DateTime.UtcNow
}));

app.MapControllers();

app.Run();
