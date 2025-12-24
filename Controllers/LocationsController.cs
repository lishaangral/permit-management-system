using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_locations")]
public class LocationsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
