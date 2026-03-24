using Microsoft.AspNetCore.Mvc;

namespace ProjetoFacul.Controllers
{
    public class AreaPublicaController : Controller
    {
        public IActionResult PageProdutos()
        {
            return View();
        }

        public IActionResult PageHome()
        {
            return View();
        }

        public IActionResult PageUnidade()
        {
            return View();
        }
    }
}
