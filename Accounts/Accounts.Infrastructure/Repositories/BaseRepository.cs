using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Pagination;
using Accounts.Infrastructure.Entities;
using Accounts.Infrastructure.Extensions;
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
        protected readonly string _dbConnectionString;
        protected string TableName { get; set; }

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StocksConnection");
            _dbConnectionString = SetDbConnectionString(_connectionString);
        }

        private string SetDbConnectionString(string connectionString)
        {
            return connectionString.Replace("master", "StocksDB");
        }

        private async Task<bool> CheckIfDbExistsAsync()
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DECLARE @MyTableVar table([test] [varchar](250)); " +
                    "IF DB_ID('StocksDB') IS NOT NULL INSERT INTO @MyTableVar (test) values('db exists') " +
                    "ELSE INSERT INTO @MyTableVar (test) values('no') " +
                    "SELECT * FROM @MyTableVar", connection);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            if (dataTable.Rows[0]["test"].ToString() == "db exists")
                return true;

            return false;
        }

        protected virtual async Task CreateDbIfNotExist()
        {
            if (await CheckIfDbExistsAsync())
            {
                return;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CreateStockDb", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                await cmd.ExecuteNonQueryAsync();
            }
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

            return table
                .AsEnumerable()
                .Select(x => DataRowToEntity<TOutput>(x))
                .ToList();
        }

        public virtual async Task<TOutput> InsertAsync<TInput, TOutput>(TInput entity)
            where TOutput : new()
        {
            await CreateDbIfNotExist();

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
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(
                    "USE StocksDB; DECLARE @MyTableVar table([testID] [uniqueidentifier]); " +
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
            await CreateDbIfNotExist();

            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($@"SELECT * FROM {TableName} WHERE Id = @Id", connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return DataRowToEntity<TOutput>(dataTable.Rows[0]);
        }

        public virtual async Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber, int pageSize)
            where TOutput : new()
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"USE StocksDB; SELECT * FROM {TableName}", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable
                .AsEnumerable()
                .Select(x => DataRowToEntity<TOutput>(x))
                .ToList()
                .Paginate(pageNumber, pageSize);
        }

        public async Task<TOutput> UpdateAsync<TInput, TOutput>(Guid id, TInput entity)
            where TOutput : new()
        {
            await CreateDbIfNotExist();

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

            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"DECLARE @MyTableVar table([testID] [uniqueidentifier]); " +
                    $"USE StocksDB; UPDATE {TableName} SET {valuesToProps} WHERE Id = '{id}'" +
                    $"OUTPUT INSERTED.Id INTO @MyTableVar " +
                    $"SELECT * FROM {TableName} WHERE Id = CAST((SELECT TOP 1 testID from @MyTableVar) AS nvarchar(250))", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return DataRowToEntity<TOutput>(dataTable.Rows[0]);
        }

        public async Task DeleteAsync(Guid id)
        {
            await CreateDbIfNotExist();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM {TableName} WHERE Id = '{id}'", connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }      
    }
}
