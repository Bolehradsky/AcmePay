using _Common.Exceptions;
using AcmePay.Infrastructure.Database;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;

namespace AcmePay.Infrastructure.Repository.Generics
{
    public class GenericRepository<T, TId> : IGenericRepository<T, TId>
        where T : class
        where TId : struct, IEquatable<TId>
    {
        IDbConnection _connection;



        public GenericRepository(SqlConnectionProvider connectionProvider)
        {
            _connection = connectionProvider.GetConnection();
        }

        public async Task<bool> Add(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = this.GetTableName();
                string columns = this.GetColumns(excludeKey: true);
                string properties = this.GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                rowsEffected = await _connection.ExecuteAsync(query, entity);
            }
            catch (Exception exception)
            {
                throw new DatabaseException($"{MethodBase.GetCurrentMethod()} method exception: {exception.Message}");
            }
            return rowsEffected > 0 ? true : false;
        }

        public async Task<bool> Delete(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = this.GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = this.GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE Id = @{keyProperty}";

                rowsEffected = await _connection.ExecuteAsync(query, entity);
            }
            catch (Exception exception)
            {
                throw new DatabaseException($"{MethodBase.GetCurrentMethod()} method exception: {exception.Message}");
            }

            return rowsEffected > 0 ? true : false;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T>? result = null;
            try
            {
                string tableName = this.GetTableName();
                string query = $"SELECT * FROM {tableName}";

                result = _connection.Query<T>(query);
            }
            catch (Exception exception)
            {
                throw new DatabaseException($"{MethodBase.GetCurrentMethod()} method exception: {exception.Message}");
            }

            return result;
        }

        public async Task<T> GetById(TId Id)
        {
            T? result = null;
            try
            {
                string tableName = this.GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE  Id = '{Id}'";
                result = await _connection.QueryFirstOrDefaultAsync<T>(query, new { Id = Id.ToString() });
            }
            catch (Exception exception)
            {
                throw new DatabaseException($"{MethodBase.GetCurrentMethod()} method exception: {exception.Message}");
            }
            if (result is null)
            {
                throw new EntityNotFoundException("Transaction not found!");
            }
            return result;
        }

        public async Task<bool> Update(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = this.GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = this.GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == "Id") continue;
                    query.Append($"{property.Name} = @{property.Name},");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE Id = @Id");

                rowsEffected = await _connection.ExecuteAsync(query.ToString(), entity);
            }
            catch (Exception exception)
            {
                throw new DatabaseException($"{MethodBase.GetCurrentMethod()} method exception: {exception.Message}");
            }

            return rowsEffected > 0 ? true : false;
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return $"[{type.Name}]";
        }

        public static string? GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute? columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute!.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }


        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties();

            //var properties = typeof(T).GetProperties()
            //    .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties().ToList()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string? GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }


    }
}