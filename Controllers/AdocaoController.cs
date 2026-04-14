using Microsoft.AspNetCore.Mvc;

namespace ProjetoFacul.Controllers
{
    public class AdocaoController : Controller
    {
        public IActionResult MinhasAdocoes()
        {
            return View();
        }

        public IActionResult Solicitar(int petId)
        {
            return View();
        }

    }
}
