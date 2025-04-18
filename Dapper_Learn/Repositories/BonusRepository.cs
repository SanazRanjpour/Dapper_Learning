﻿using Dapper;
using Dapper_Learn.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

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

            if (id != 0)
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


        public async Task<Company> GetCompanyWithEmployees(int id)
        {
            // First define a parameter with name of CompanyId
            var param = new
            {
                CompanyId = id
            };
            // The @CompanyId is the param we made
            var query = @"SELECT * FROM Companies WHERE CompanyId = @CompanyId;
                          SELECT * FROM Employees WHERE CompanyId = @CompanyId;";
            Company company;

            // Now Dapper should control our Multiple-Results
            // Without using, objects like QueryMultiple (that hold resources) would need
            // to be manually disposed by calling lists.Dispose().
            using (var lists = await db.QueryMultipleAsync(query, param))
            {
                company = (await lists.ReadAsync<Company>()).ToList().FirstOrDefault();
                company.Employees = (await lists.ReadAsync<Employee>()).ToList();
            }
            return company;
        }




        /// ****** Description Of The GetAllCompaniesWithEmployees Method ******
        /// <summary>
        /// Retrieves a list of companies, each with their associated employees, from the database.
        /// This method performs the following tasks:
        /// 1. Fetches company and employee data using an SQL query with an INNER JOIN.
        /// 2. Uses a Dictionary to ensure companies are added to the result only once.
        /// 3. Associates employees with their corresponding company.
        /// 4. Returns a distinct list of companies with their related employees.
        /// 5. By the way, The 'out' keyword tells the TryGetValue method of Dictionary to assign the value 
        ///    associated with the key to the variable currentCompany if the key is found.
        /// </summary>
        /// <returns>
        /// A list of companies, each containing their associated employees.
        /// </returns>

        public async Task<List<Company>> GetAllCompaniesWithEmployees()
        {
            string query = @"SELECT C.*, E.* FROM Employees AS E INNER JOIN Companies AS C 
                             ON E.CompanyId = C.CompanyId";

            var companyDic = new Dictionary<int, Company>();

            var company = await db.QueryAsync<Company, Employee, Company>(query, (c, e) =>
            {
                if (!companyDic.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.CompanyId, currentCompany);
                }

                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn: "EmployeeId");

            return company.Distinct().ToList();
        }

        public async Task AddRecordsToCompany(Company company)
        {
            var query = @"INSERT INTO Companies
                          (Name, Address, City, State, PostalCode)
                          VALUES (@Name, @Address, @City, @State, @PostalCode);
                          SELECT CAST (SCOPE_IDENTITY() AS INT);";

            var id = await db.QueryFirstOrDefaultAsync<int>(query, company);
            company.CompanyId = id;

            /// ***************** First Solution ****************

            //foreach (var employee in company.Employees)
            //{

            //    employee.CompanyId = company.CompanyId;

            //    var queryEm = @"INSERT INTO Employees
            //                    (Name, Email, Phone, Title, CompanyId)
            //                    VALUES (@Name, @Email, @Phone, @Title, @CompanyId);
            //                    SELECT CAST (SCOPE_IDENTITY() AS INT);";

            //    await db.QueryFirstOrDefaultAsync<int>(queryEm, employee);

            //}

            /// ***************** End Of First Solution ******************

            /// ******************* Second Solution **********************

            company.Employees.Select(e =>
            {
                e.CompanyId = id;
                return e;
            }).ToList();

            var queryEm = @"INSERT INTO Employees
                            (Name, Email, Phone, Title, CompanyId)
                            VALUES (@Name, @Email, @Phone, @Title, @CompanyId);
                            SELECT CAST (SCOPE_IDENTITY() AS INT);";

            await db.ExecuteAsync(queryEm, company.Employees);

            /// ******************* End Of Second Solution **********************
        }

        public async Task AddTestRecordsToCompanyWithTransaction(Company company)
        {
            using (var trans = new TransactionScope())
            {
                try
                {
                    var query = @"INSERT INTO Companies
                          (Name, Address, City, State, PostalCode)
                          VALUES (@Name, @Address, @City, @State, @PostalCode);
                          SELECT CAST (SCOPE_IDENTITY() AS INT);";

                    var id = await db.QueryFirstOrDefaultAsync<int>(query, company);
                    company.CompanyId = id;

                    company.Employees.Select(e =>
                    {
                        e.CompanyId = id;
                        return e;
                    }).ToList();

                    var queryEm = @"INSERT INTO Employees1
                            (Name, Email, Phone, Title, CompanyId)
                            VALUES (@Name, @Email, @Phone, @Title, @CompanyId);
                            SELECT CAST (SCOPE_IDENTITY() AS INT);";

                    await db.ExecuteAsync(queryEm, company.Employees);

                    trans.Complete();

                    /// If try is not completed and the 'transaction.Completed' doesn't meet,
                    /// the RollBack is done and none of the insertions are done.
                   
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task RemoveCompany(int companyId)
        {
            var query = @"DELETE FROM Employees 
                          WHERE CompanyId = @CompanyId
                         
                          DELETE FROM Companies WHERE CompanyId = @CompanyId";
            await db.ExecuteAsync(query, new { @CompanyId = companyId });

        }

        public async Task<List<Company>> Search(string param)
        {
            var query = @"SELECT * FROM Companies
                          WHERE Name LIKE '%' + @param + '%'";

            return (await db.QueryAsync<Company>(query, new { param })).ToList();
        }

       
    }
}
