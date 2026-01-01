using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAPIServices(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);
// Add Serilog
builder.Host.UseSerilog((context, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration) // Read from appsettings.json
);
var app = builder.Build();

app.UseOrderingAPIServices();
app.Run();