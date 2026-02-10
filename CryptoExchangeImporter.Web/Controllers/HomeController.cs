using System.Diagnostics;

using CryptoExchangeImporter.Web.Models;

using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeImporter.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
