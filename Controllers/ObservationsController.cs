using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ObservationsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult Create()
    {
        return View();
    }
}
