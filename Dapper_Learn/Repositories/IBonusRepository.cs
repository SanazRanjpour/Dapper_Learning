using Dapper_Learn.Models;

namespace Dapper_Learn.Repositories
{
    public interface IBonusRepository
    {
        Task<List<Employee>> GetEmployeesWithCompany(int id);
        Task<Company> GetCompanyWithEmployees(int id);
    }
}
