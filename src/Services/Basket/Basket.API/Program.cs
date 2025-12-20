using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException());
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    // Fix the excessive schema checking
    options.AutoCreateSchemaObjects = builder.Environment.IsDevelopment()
        ? AutoCreate.CreateOrUpdate
        : AutoCreate.None;
}).UseLightweightSessions();

// Add Serilog
builder.Host.UseSerilog((context, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration) // Read from appsettings.json
);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// builder.Services.AddScoped<IBasketRepository>(provider =>
// {
//     var basketRepository = provider.GetRequiredService<IBasketRepository>();
//     return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
// });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException())
    .AddRedis(builder.Configuration.GetConnectionString("Redis") ?? 
              throw new InvalidOperationException());

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.Run();