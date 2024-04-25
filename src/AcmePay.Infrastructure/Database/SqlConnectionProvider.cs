using Microsoft.Data.SqlClient;

namespace AcmePay.Infrastructure.Database
{
    public class SqlConnectionProvider
    {
        private readonly string connectioString;

        public SqlConnectionProvider(string connectioString)
        {
            this.connectioString = connectioString;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(connectioString);
        }
    }
}
