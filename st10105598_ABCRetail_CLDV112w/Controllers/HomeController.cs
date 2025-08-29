using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using st10105598_ABCRetail_CLDV112w.Models;
using System.Diagnostics;

namespace st10105598_ABCRetail_CLDV112w.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        public IActionResult Index()
        {
            logger.LogInformation("Index page visited");
            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
