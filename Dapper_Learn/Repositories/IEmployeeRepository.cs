using Dapper_Learn.Models;

namespace Dapper_Learn.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> Get(int? id);
        Task<List<Employee>> GetAll();
        Task<Employee> Add(Employee Employee);
        Task Update(Employee Employee);
        Task Delete(int? id);
        //Task SaveChangesAsync();
    }
}
