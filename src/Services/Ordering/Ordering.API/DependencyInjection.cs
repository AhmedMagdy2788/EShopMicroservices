using BuildingBlocks.Exceptions;
using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Add Services to the container
        services.AddCarter();
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);


        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static WebApplication UseOrderingAPIServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();
        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}