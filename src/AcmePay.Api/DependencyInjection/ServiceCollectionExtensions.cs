using Mapster;

namespace AcmePay.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddCommonMapsterProfiles();
        return services;
    }


    public static IServiceCollection AddCommonMapsterProfiles(this IServiceCollection services)
    {
        /// Generalne  definicije za primitvne tipove, cije mapiranje cesto ponavljam pa

        //TypeAdapterConfig<DateOnly, DateTime>.NewConfig().MapWith(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue));
        //TypeAdapterConfig<DateOnly?, DateTime?>.NewConfig().MapWith(
        //    dateOnly => dateOnly == null ? null : dateOnly.Value.ToDateTime(TimeOnly.MinValue));

        //TypeAdapterConfig<DateTime, DateOnly>.NewConfig().MapWith(dateTime => DateOnly.FromDateTime(dateTime));
        //TypeAdapterConfig<DateTime?, DateOnly?>.NewConfig().MapWith(
        //    dateTime => dateTime == null ? null : DateOnly.FromDateTime(dateTime.Value));

        //var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        //typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);

        return services;
    }

}
