using Dapper_Learn.Models;
using Dapper_Learn.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dapper_Learn.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IBonusRepository _bonusRepository) : Controller
    {
        public async Task<IActionResult> Index(string q)
        {
            if(!string.IsNullOrEmpty(q))
            {
               return View(await _bonusRepository.Search(q));
            }
            return View(await _bonusRepository.GetAllCompaniesWithEmployees());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> AddRecords()
        {
            Company company = new Company
            {
                Name = "Test Company" + Guid.NewGuid().ToString(),
                Address = "Test Address",
                City = "Test City",
                State = "Test State",
                PostalCode = "Test PostalCode"
            };

            company.Employees.Add(new Employee
            {
                Name = "Test Employee Name" + Guid.NewGuid().ToString(),
                Email = "Test Email",
                Phone = "Test Phone",
                Title = "Test Title"
            });

            company.Employees.Add(new Employee
            {
                Name = "Test Employee Name 2" + Guid.NewGuid().ToString(),
                Email = "Test Email 2",
                Phone = "Test Phone 2",
                Title = "Test Title 2"
            });

            //await _bonusRepository.AddRecordsToCompany(company);
            await _bonusRepository.AddTestRecordsToCompanyWithTransaction(company);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> RemoveRecords (int id)
        {
            await _bonusRepository.RemoveCompany(id);
            return RedirectToAction("Index");
        }
    }
}
