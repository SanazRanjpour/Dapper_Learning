using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class CompanyRepositoryDapper : ICompanyRepository
    {
        private readonly IDbConnection db;
        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<Company?> Get(int? id)
        {
            string query = " SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return await
                db.QueryFirstOrDefaultAsync<Company>(query, new { CompanyId = id });
        }

        public async Task<List<Company>> GetAll()
        {
            string query = "SELECT * FROM Companies";
            var companies = await db.QueryAsync<Company>(query);
            return companies.ToList();

        }

        public async Task<Company> Add(Company company)
        {
            string query = 
                "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES (@Name, @Address, @City, @State, @PostalCode);" 
               + "SELECT CAST(SCOPE_IDENTITY() as INT)";
            var id = await db.QueryFirstOrDefaultAsync<int>(query, new
            {
                Name = company.Name,
                Address = company.Address,
                City = company.City,
                State = company.State,
                PostalCode = company.PostalCode
            });
            company.CompanyId = id;
            return company;
        }


        // Not required for Dapper by default. Handle transactions directly.
        //public Task SaveChangesAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task Update(Company company)
        {
            string query =
                @"UPDATE Companies SET
                 Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode
                 WHERE CompanyId = @CompanyId";
            await db.ExecuteAsync(query, new
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Address = company.Address,
                City = company.City,
                State = company.State,
                PostalCode = company.PostalCode
            });
                
        }


        public async Task Delete(int? id)
        {
            string query = "DELETE FROM Companies WHERE CompanyId = @CompanyId";
            await db.ExecuteAsync(query, new {CompanyId = id});
           // or  await db.ExecuteAsync(query, new { id });
        }
    }
}

