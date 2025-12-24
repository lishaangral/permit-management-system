using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PermitsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = "create_permits")]
    public IActionResult Create()
    {
        return View();
    }
}
