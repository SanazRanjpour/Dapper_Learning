using Dapper_Learn.Models;

namespace Dapper_Learn.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> Get(int? id);
        Task<List<Company>> GetAll();
        Task<Company> Add(Company company);
        Task Update(Company company);
        Task Delete(int? id);
        //Task SaveChangesAsync();
    }
}
