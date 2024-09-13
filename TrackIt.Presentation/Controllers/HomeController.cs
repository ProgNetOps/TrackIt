using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrackIt.Presentation.Models;

namespace TrackIt.Presentation.Controllers
{
    [AllowAnonymous]
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        [ViewData]
        public string Title { get; set; } = string.Empty;
        [ViewData]
        public string PageHeader { get; set; } = string.Empty;


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            Title = "Home";
            return View();
        }


        [HttpGet]
        public IActionResult Welcome()
        {
            Title = "Welcome";
            PageHeader = "Track It";

            return View();
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
