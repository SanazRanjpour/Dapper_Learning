using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class BonusRepository(IConfiguration configuration) : IBonusRepository
    {
        private IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        public async Task<Company> GetCompanyWithEmployees(int id)
        {
            // First define a parameter with name of CompanyId
            var param = new
            {
                CompanyId = id
            };
            // The @CompanyId is the param we made
            var query = @"SELECT * FROM Companies WHERE CompanyId = @CompanyId;
                         SELECT * FROM Employees WHERE CompanyId = @CompanyId";
            Company company;

            // Now Dapper should control our Multiple-Results
            using(var lists = await db.QueryMultipleAsync(query, param))
            {
                company = (await lists.ReadAsync<Company>()).ToList().FirstOrDefault();
                company.Employees = (await lists.ReadAsync<Employee>()).ToList();
            }
            return company;
        }

        public async Task<List<Employee>> GetEmployeesWithCompany(int id)
        {
            var query = @"SELECT E.*, C.*
                  FROM Employees AS E
                  INNER JOIN Companies AS C
                  ON E.CompanyId = C.CompanyId";

            if(id != 0)
            {
                query += " WHERE E.CompanyId = @Id";  // Before WHERE Must Be A Space
            }
            var employees = await db.QueryAsync<Employee, Company, Employee>(query, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { id }, splitOn: "CompanyId");
            //return [.. employees]; 
             return employees.ToList();
        }

    }
}
