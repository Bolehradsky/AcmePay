using AcmePay.Api.DependencyInjection;
using AcmePay.Api.Midleware;
using AcmePay.Application.DependencyInjection;
using AcmePay.Infrastructure.DependencyInjection;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

try
    {

    builder.WebHost.ConfigureKestrel(
    serverOptions =>
    {

        serverOptions.ListenAnyIP(7155);
        serverOptions.ListenAnyIP(7156, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });


    Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
    builder.Logging.AddSerilog(Log.Logger);

    Log.Information("Starting web host");

    builder.Services.AddControllers();

    builder.Services
            .AddPresentationLayer()
            .AddApplicationLayer()
            .AddInfrastructureLayer(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options => options.AddPolicy("allowAll",
                                                          policy => policy.AllowAnyOrigin()
                                                              .AllowAnyHeader()
                                                              .AllowAnyMethod()));

    var app = builder.Build();


    if (app.Environment.IsDevelopment())
        {
        app.UseSwagger();
        app.UseSwaggerUI();
        }

    app.UseCors("allowAll");
    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandler>();


    app.MapControllers();

    app.Run();
    }
catch (Exception ex)
    {
    Log.Information("Starting web host exception " + ex.Message);
    }
finally
    {
    Log.CloseAndFlush();
    }
