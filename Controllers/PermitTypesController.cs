using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_permit_types")]
public class PermitTypesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
