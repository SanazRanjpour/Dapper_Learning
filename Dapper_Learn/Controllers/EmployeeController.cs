using Dapper_Learn.Models;
using Dapper_Learn.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dapper_Learn.Controllers
{
    public class EmployeeController
        (IEmployeeRepository _employeeRepository,
        ICompanyRepository _companyRepository,
        IBonusRepository _bonusRepository) : Controller
    {
        [BindProperty]
        public Employee Employee { get; set; }
        
        public async Task<IActionResult> Index()
        {
            return View(await _bonusRepository.GetEmployeesWithCompany());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CompanyId = new SelectList(await _companyRepository.GetAll(), "CompanyId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Create))]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.Add(Employee);
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(await _companyRepository.GetAll(), "CompanyId", "Name");
            return View(Employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee = await _employeeRepository.Get(id.Value);
            ViewBag.CompanyId = new SelectList
                               (await _companyRepository.GetAll(), "CompanyId", "Name", Employee.CompanyId);
            if (Employee == null)
            {
                return NotFound();
            }
          
            return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeRepository.Update(Employee);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency error: {ex.Message}");
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList
                              (await _companyRepository.GetAll(), "CompanyId", "Name", Employee.CompanyId);
            return View(Employee);
        }

      
        [HttpGet]
        public async Task<IActionResult> Delete (int? id)
        {
            if (id == null) return NotFound();
            Employee = await _employeeRepository.Get(id.Value);
            if (Employee == null) return NotFound();
            return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed (int id)
        {
           await _employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
