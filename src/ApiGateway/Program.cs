using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

var app = builder.Build();

app.Use((context, next) =>
{
    foreach (var (key, value) in context.Request.Headers)
    {
        Console.WriteLine($"{key}: {value}");
    }

    return next();
});

app.UseForwardedHeaders();

app.MapReverseProxy();

app.Run();