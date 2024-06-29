using Microsoft.AspNetCore.Mvc;

namespace Swallow.Controllers;

public class TripController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}