using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class BonusRepository(IConfiguration configuration) : IBonusRepository
    {
        private IDbConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
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
