using Microsoft.AspNetCore.Mvc;
using MvcCorePracticaComics.Models;
using MvcCorePracticaComics.Repositories;

namespace MvcCorePracticaComics.Controllers
{
    public class ComicsController : Controller
    {
        RepositoryComics repo;

        public ComicsController()
        {
            this.repo = new RepositoryComics();
        }
        public IActionResult Index()
        {
            List<Comic> c = this.repo.GetComics();
            return View(c);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(string nombre, string imagen, string descripcion)
        {
            await this.repo.InsertComicAsync(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorComics()
        {

            ViewData["NOMBRE"] = this.repo.GetNombresComics();
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorComics(string nombre)
        {
            ViewData["NOMBRE"] = this.repo.GetNombresComics();
            Comic c = this.repo.GetComicsNombre(nombre);
            return View(c);
        }
    }
}
