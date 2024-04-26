using _Common.Middleware;
using AcmePay.Api.DependencyInjection;
using AcmePay.Application.DependencyInjection;
using AcmePay.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

try
{


    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services
            .AddPresentationLayer()
            .AddApplicationLayer()
            .AddInfrastructureLayer(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandler>();


    app.MapControllers();

    app.Run();
}
catch (Exception)
{

}
finally
{

}
