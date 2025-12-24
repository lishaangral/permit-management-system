using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_issuer_roles")]
public class IssuerRolesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
