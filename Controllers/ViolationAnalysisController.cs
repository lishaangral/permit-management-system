using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ViolationAnalysisController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
