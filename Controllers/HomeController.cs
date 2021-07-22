namespace MessiFinder.Controllers
{
    using MessiFinder.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services.Homes;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var indexModel = this.homeService.Index();

            return View(indexModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
