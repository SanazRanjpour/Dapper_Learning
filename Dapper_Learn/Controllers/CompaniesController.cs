using Dapper;
using Dapper_Learn.Models;
using Dapper_Learn.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dapper_Learn.Controllers
{
    public class CompaniesController(ICompanyRepository _companyRepository,
                                     IBonusRepository _bonusRepository,
                                     IDapperGenericRepository _dapperGenericRepository) : Controller
    {
        
        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //var companies = await _companyRepository.GetAll();
            //return View(companies);

            return View(await _dapperGenericRepository
                             .ListAsync<Company>
                             ("usp_GetAllCompanies"));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = await _bonusRepository.GetCompanyWithEmployees(id.Value);
            var company = await _dapperGenericRepository
                         .SingleAsync<Company>
                         ("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                //await _companyRepository.Add(company);
                //await _context.SaveChangesAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", 0, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@Name", company.Name);
                parameters.Add("@Address", company.Address);
                parameters.Add("@City", company.City);
                parameters.Add("@State", company.State);
                parameters.Add("@PostalCode", company.PostalCode);
                await _dapperGenericRepository
                     .ExecuteAsync("usp_AddCompany", parameters);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = await _companyRepository.Get(id.Value);
            var company = await _dapperGenericRepository
                         .SingleAsync<Company>
                         ("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //await _companyRepository.Update(company);
                    //await _context.SaveChangesAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
                    parameters.Add("@Name", company.Name);
                    parameters.Add("@Address", company.Address);
                    parameters.Add("@City", company.City);
                    parameters.Add("@State", company.State);
                    parameters.Add("@PostalCode", company.PostalCode);
                    await _dapperGenericRepository
                          .ExecuteAsync("usp_UpdateCompany", parameters);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //if (!CompanyExists(company.CompanyId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                    // Example logging
                    Console.WriteLine($"Concurrency error: {ex.Message}");
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = await _companyRepository.Get(id.Value);
            var company = await _dapperGenericRepository
                         .SingleAsync<Company>
                         ("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            //var company = await _companyRepository.Get(id);
            var company = await _dapperGenericRepository
                         .SingleAsync<Company>
                         ("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
            if (company != null)
            {
                //await _companyRepository.Delete(id);
                await _dapperGenericRepository
                      .ExecuteAsync("usp_DeleteCompany", new { CompanyId = id });
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool CompanyExists(int id)
        //{
        //    return _context.Companies.Any(e => e.CompanyId == id);
        //}
    }
}
