using AcmePay.Domain.Repositories;
using AcmePay.Infrastructure.Database;
using AcmePay.Infrastructure.Queries;
using AcmePay.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AcmePay.Application.Persistence.Queries.ReadTransactionsModel;

namespace AcmePay.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("AcmeDatabase") ?? throw new ApplicationException("Connection string does not exist!");
                return new SqlConnectionProvider(connectionString);
            });

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IReadTransactionsQuery, ReadTransactionsQuery>();

            return services;
        }

    }
}
