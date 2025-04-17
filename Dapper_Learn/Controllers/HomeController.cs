using System.Diagnostics;
using Dapper_Learn.Models;
using Dapper_Learn.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Learn.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IBonusRepository _bonusRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
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
    }
}
