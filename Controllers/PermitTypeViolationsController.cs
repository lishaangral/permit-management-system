using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_permit_type_violations")]
public class PermitTypeViolationsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
