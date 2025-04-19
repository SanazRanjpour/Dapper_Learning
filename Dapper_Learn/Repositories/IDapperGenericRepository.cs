namespace Dapper_Learn.Repositories
{
    public interface IDapperGenericRepository
    {
        string ConnectionString { get; set; }
        Task ExecuteAsync(string name);
        Task ExecuteAsync(string name, object param);
        Task<List<T>> ListAsync<T>(string name);
        Task<List<T>> ListAsync<T>(string name, int id);
        Task<List<T>> ListAsync<T>(string name, object param);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> ListAsync<T1, T2, T3>(string name, object param);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string name, object param);
        Task QueryExecuteAsync(string name);
        Task QueryExecuteAsync(string name, object param);
        Task<T> SingleAsync<T>(string name, int id);
        Task<T> SingleAsync<T>(string name, object param);
    }
}
