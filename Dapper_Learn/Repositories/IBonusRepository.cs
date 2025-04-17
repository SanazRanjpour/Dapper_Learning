using Dapper_Learn.Models;

namespace Dapper_Learn.Repositories
{
    public interface IBonusRepository
    {
        Task<List<Employee>> GetEmployeesWithCompany(int id);
        Task<Company> GetCompanyWithEmployees(int id);
        Task<List<Company>> GetAllCompaniesWithEmployees();
        Task AddRecordsToCompany(Company company);
        Task AddTestRecordsToCompanyWithTransaction(Company company);
        Task RemoveCompany(int companyId);
        Task<List<Company>> Search(string param);
    }
}
