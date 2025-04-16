using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class BonusRepository(IConfiguration configuration) : IBonusRepository
    {
        private IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        public async Task<List<Employee>> GetEmployeesWithCompany()
        {
            var query = @"SELECT E.*, C.*
                  FROM Employees AS E
                  INNER JOIN Companies AS C
                  ON E.CompanyId = C.CompanyId";
            var employees = await db.QueryAsync<Employee, Company, Employee>(query, (e, c) =>
            {
                e.Company = c;
                return e;
            }, splitOn: "CompanyId");
            return employees.ToList();
        }

    }
}
