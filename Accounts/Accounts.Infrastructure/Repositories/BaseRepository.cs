using Accounts.Domain.Abstraction.Repositories;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Accounts.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository
        where TEntity : BaseEntity
    {
        protected readonly string _connectionString;
        protected string TableName { get; set; }

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StocksConnection");
        }

        protected virtual TOutput DataRowToEntity<TOutput>(DataRow dataRow)
            where TOutput : new()
        {
            return new TOutput();
        }

        protected virtual List<TOutput> DataTableToCollection<TOutput>(DataTable table)
            where TOutput : new()
        {
            if (table == null)
            {
                return null;
            }

            return table.AsEnumerable().Select(x => DataRowToEntity<TOutput>(x)).ToList();
        }

        public virtual async Task<TOutput> InsertAsync<TInput, TOutput>(TInput entity)
            where TOutput : new()
        {
            var properties = typeof(TInput).GetProperties().Where(x => x.CanRead).ToArray();
            string columnNames = string.Join(", ", properties.Select(x => x.Name));
            StringBuilder values = new StringBuilder();

            for (int i = 0; i < properties.Count(); i++)
            {
                if (properties[i].PropertyType == typeof(string) || properties[i].PropertyType == typeof(Guid))
                {
                    values.Append($"'{properties[i].GetValue(entity)}'");
                }
                else
                {
                    values.Append($"{properties[i].GetValue(entity)}");
                }

                if (i + 1 < properties.Count())
                {
                    values.Append(',');
                }
            }

            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(
                    "DECLARE @MyTableVar table([testID] [uniqueidentifier]); " +
                    $"INSERT INTO {TableName} ({columnNames}) " +
                    "OUTPUT INSERTED.Id INTO @MyTableVar " +
                    $"VALUES ({values}) " +
                    $"SELECT * FROM {TableName} WHERE Id = CAST((SELECT TOP 1 testID from @MyTableVar) AS nvarchar(250))", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return DataRowToEntity<TOutput>(dataTable.Rows[0]);
        }

        public virtual async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
            where TOutput : new()
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {TableName} WHERE Id = '{id}'", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return DataRowToEntity<TOutput>(dataTable.Rows[0]);
        }

        public virtual async Task<List<TOutput>> GetAllAsync<TOutput>()
            where TOutput : new()
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {TableName}", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable.AsEnumerable().Select(x => DataRowToEntity<TOutput>(x)).ToList();
        }

        public async Task UpdateAsync<TInput>(Guid id, TInput entity)
        {
            var properties = typeof(TInput).GetProperties().Where(x => x.CanRead).ToArray();
            StringBuilder valuesToProps = new StringBuilder();

            for (int i = 0; i < properties.Count(); i++)
            {
                if (properties[i].PropertyType == typeof(string))
                {
                    valuesToProps.Append(properties[i].Name + "= " + $"'{properties[i].GetValue(entity)}'");
                }
                else
                {
                    valuesToProps.Append(properties[i].Name + "= " + properties[i].GetValue(entity));
                }

                if (i + 1 < properties.Count())
                {
                    valuesToProps.Append(',');
                }
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE {TableName} SET ({valuesToProps})", connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM {TableName} WHERE Id = '{id}'", connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }      
    }
}
