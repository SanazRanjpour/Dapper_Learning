using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class CompanyRepositoryDapperSP : ICompanyRepository
    {
        private IDbConnection db;
        public CompanyRepositoryDapperSP(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Company?> Get(int? id)
        {
            return await
                 db.QueryFirstOrDefaultAsync<Company>("usp_GetCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Company>> GetAll()
        {
            var companies = await db.QueryAsync<Company>("usp_GetAllCompanies", commandType: CommandType.StoredProcedure);
            return companies.ToList();
        }

        public async Task<Company> Add(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, ParameterDirection.Output);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            await db.ExecuteAsync("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");
            return company;

        }


        public async Task Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            await db.ExecuteAsync("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);

        }
        public async Task Delete(int? id)
        {
            await db.ExecuteAsync("usp_DeleteCompany", new {CompanyId =  id});
        }


    }
}
