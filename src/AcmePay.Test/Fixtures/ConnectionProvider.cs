

using AcmePay.Infrastructure.Database;

namespace AcmePay.Test.Fixtures
{
    public static class ConnectionProvider
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=acme01;Trusted_Connection=True;";
        public static SqlConnectionProvider Connect()
        {
            return new SqlConnectionProvider(connectionString);
        }
    }
}
