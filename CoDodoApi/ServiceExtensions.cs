using CoDodoApi.Services;
using Microsoft.AspNetCore.Authentication;
using Serilog;

namespace CoDodoApi;

public static class ServiceExtensions
{
    public static
    WebApplicationBuilder AddConfiguredSerilog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host
            .UseSerilog((context, services, config) => config
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}")
                .Enrich.FromLogContext()
                .MinimumLevel.Warning());

        return builder;
    }

    public static
    IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static
    IServiceCollection AddConfiguredCors(this IServiceCollection services)
    {
        services.AddCors(o => o
            .AddDefaultPolicy(p => p
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

        return services;
    }

    public static
    IServiceCollection AddConfiguredAuthentication(
        this IServiceCollection services)
    {
        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", null);

        return services;
    }
}
