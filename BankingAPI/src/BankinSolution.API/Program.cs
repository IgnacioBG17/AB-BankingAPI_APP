using BankingSolution.Application;
using BankingSolution.Application.Features.Accounts.Querys.GetBalanceByAccount;
using BankingSolution.Infrastructure;
using BankingSolution.Infrastructure.Persistence;
using BankinSolution.API;
using BankinSolution.API.Middlewares;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* ConnectionString */
builder.Services.AddDbContext<BankingSolutionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(BankingSolutionDbContext).Assembly.FullName)
    )
);

/* registro de dependencias */
builder.Services.AddWebApi()
                .AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices(builder.Configuration);

/* Referencia de comunicacion MediatR para utilizar el patron CQRS */
builder.Services.AddMediatR(typeof(GetBalanceByAccountQueryHandler).Assembly);

builder.Services.AddControllers();
var app = builder.Build();

/* Swagger */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); /* Middleware Personalizado */
app.UseAuthorization();
app.MapControllers();

/* Cargar la data inicial en la BD */
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = services.GetRequiredService<BankingSolutionDbContext>();
        await context.Database.MigrateAsync();
        await BankingSolutionDbContextData.LoadDataAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error al ejecutar la migración o la carga inicial de datos");
    }
}

app.Run();
