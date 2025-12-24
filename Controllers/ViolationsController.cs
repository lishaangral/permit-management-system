using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_violations")]
public class ViolationsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
