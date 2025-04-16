using Dapper;
using Dapper.Contrib.Extensions;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class CompanyRepositoryDapperContrib(IConfiguration configuration) : ICompanyRepository
    {
        private IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        public async Task<Company?> Get(int? id)
        {
            return await
                 db.GetAsync<Company>(id);
        }

        public async Task<List<Company>> GetAll()
        {
            var companies = await db.GetAllAsync<Company>();
            return companies.ToList();
        }

        public async Task<Company> Add(Company company)
        {
            var id = await db.InsertAsync<Company>(company);
            company.CompanyId = id;
            return company;
        }


        public async Task Update(Company company)
        {
            await db.UpdateAsync(company);
        }
        public async Task Delete(int? id)
        {
            await db.DeleteAsync(new Company() { CompanyId = (int)id });
        }


    }
}
