using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PermitAnalysisController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
