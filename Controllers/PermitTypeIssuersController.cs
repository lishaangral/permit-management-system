using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "manage_permit_type_issuers")]
public class PermitTypeIssuersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
