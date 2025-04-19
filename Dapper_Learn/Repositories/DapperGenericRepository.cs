
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class DapperGenericRepository : IDapperGenericRepository
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; set; }

        public DapperGenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public async Task ExecuteAsync(string name)
        {
           await ExecuteAsync(name, null);
        }

        public async Task ExecuteAsync(string name, object param)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(name, param, commandType: CommandType.StoredProcedure);
            }

            /// Or the siple form is this: 
            ///  using var connection = CreateConnection();
           ///   await connection.ExecuteAsync(name, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<T>> ListAsync<T>(string name)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<T>(name, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.AsList();
            }

            return new List<T>();

        }

        public async Task<List<T>> ListAsync<T>(string name, int id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<T>(name, new { id }, commandType: CommandType.StoredProcedure);
               if (result != null)
                return result.AsList();
            }

            return new List<T>();

            /// Or just: 
            /// return await List<T> (name, new {id});
            
        }

        public async Task<List<T>> ListAsync<T>(string name, object param)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.AsList();
            }

            return new List<T>();
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> ListAsync<T1, T2, T3>(string name, object param)
        {
            using (var connection = CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(name, param, commandType: CommandType.StoredProcedure))
            {
                var item1 = (await multi.ReadAsync<T1>()).ToList();
                var item2 = (await multi.ReadAsync<T2>()).ToList();
                var item3 = (await multi.ReadAsync<T3>()).ToList();

                if (item1 != null && item2 != null && item3 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
                }
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(
                new List<T1>(),
                new List<T2>(),
                new List<T3>()
            );
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string name, object param)
        {
            using (var connection = CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(name, param, commandType: CommandType.StoredProcedure))
            {
                var item1 = (await multi.ReadAsync<T1>()).ToList();
                var item2 = (await multi.ReadAsync<T2>()).ToList();
                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);

                /// Or just: return Tuple.Create(item1, item2);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public async Task QueryExecuteAsync(string name)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(name, null, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task QueryExecuteAsync(string name, object param)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(name, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<T> SingleAsync<T>(string name, int id)
        {
            using (var connection = CreateConnection())
            {
               return await connection.QuerySingleOrDefaultAsync<T>(name, new {id}, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<T> SingleAsync<T>(string name, object param)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(name, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
