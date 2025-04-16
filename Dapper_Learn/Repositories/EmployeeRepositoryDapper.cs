using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Learn.Repositories
{
    public class EmployeeRepositoryDapper : IEmployeeRepository
    {
        private IDbConnection db;
        public EmployeeRepositoryDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }


        public async Task<Employee?> Get(int? id)
        {
            var query = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
            var employee = await db.QueryFirstOrDefaultAsync<Employee>(query, new { EmployeeId = id });
            return employee;
        }

        public async Task<List<Employee>> GetAll()
        {
            var query = "SELECT * FROM Employees";
            var employees = await db.QueryAsync<Employee>(query);
            return employees.ToList();
        }

        public async Task<Employee> Add(Employee Employee)
        {
            var query = @"INSERT INTO Employees (Name, Email, Phone, Title, CompanyId)
                         VALUES (@Name, @Email, @Phone, @Title, @CompanyId);
                          SELECT CAST(SCOPE_IDENTITY() as INT)";
            var id = await db.QueryFirstOrDefaultAsync<int>(query, new
            {
                Name = Employee.Name,
                Email = Employee.Email,
                Phone = Employee.Phone,
                Title = Employee.Title,
                CompanyId = Employee.CompanyId
            });
            Employee.EmployeeId = id;
            return Employee;
        }

        public async Task Update(Employee Employee)
        {
            var query =
                @"UPDATE Employees
                  SET Name = @Name, Email = @Email, Phone = @Phone, Title = @Title, CompanyId = @CompanyId
                  WHERE EmployeeId = @EmployeeId";
            await db.ExecuteAsync(query, new 
            {
                EmployeeId = Employee.EmployeeId,
                Name = Employee.Name,
                Email = Employee.Email,
                Phone = Employee.Phone,
                Title = Employee.Title,
                CompanyId = Employee.CompanyId
            });
        }
        public async Task Delete(int? id)
        {
            var query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
            await db.ExecuteAsync(query, new { EmployeeId = id });
        }

      
    }
}
