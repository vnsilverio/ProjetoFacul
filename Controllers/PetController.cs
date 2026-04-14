using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjetoFacul.Models;

namespace ProjetoFacul.Controllers
{
    public class PetController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public PetController(ILogger<PetController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
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
