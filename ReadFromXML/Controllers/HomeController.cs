using Microsoft.AspNetCore.Mvc;
using ReadFromXML.Helpers;
using ReadFromXML.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace ReadFromXML.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new MessageViewModel();
            return View(model);
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

        [HttpPost]
        public IActionResult Open(IFormFile file)
        {
            var model = new MessageViewModel();
            model.Messages = XMLHelper.Load(file);
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Add(Message message)
        {
            ModelState.Clear();
            var model = new MessageViewModel();
            model.Messages = XMLHelper.Add(message);
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            ModelState.Clear();
            var model = new MessageViewModel();
            model.Messages = XMLHelper.Delete(Id);
            return View("Index", model);
        }
    }
}