using _Common.Utils;
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
        TypeAdapterConfig<Guid, string>.NewConfig().MapWith(src => EncryptGuid.Encrypt(src));
        TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);

        return services;
    }

}
