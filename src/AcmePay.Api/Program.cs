using _Common.Middleware;
using AcmePay.Application.DependencyInjection;
using AcmePay.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
try
{


    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services
          .AddApplicationLayer()
          .AddInfrastructureLayer(builder.Configuration)
           ;
    // builder.Services.AddSingleton<ErrorHandler>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    //logger.Fatal("Service failed to start and threw {exception}", exception);
    // Ako ovde javi ovu gresku
    // Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType:
    // MediatR.IRequestHandler`2[Kobre.Services.Books.Application.UseCases.Commands.Book.CreateBook
    // onda nisam registrovao nesto u dependency Injection naprimer
    // services.AddScoped<IPublisherRepository, PublisherRepository>(); U Infrastructuri
}
finally
{
    // Log.CloseAndFlush();
}
