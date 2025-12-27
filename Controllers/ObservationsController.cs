//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//[Authorize]
//public class ObservationsController : Controller
//{
//    public IActionResult Index()
//    {
//        return View();
//    }

//    [Authorize(Policy = "create_observations")]
//    public IActionResult Create()
//    {
//        return View();
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PemitManagement.ViewModels.Observations;

[Authorize]
public class ObservationsController : Controller
{
    [Authorize(Policy = "create_observations")]
    public IActionResult Create()
    {
        var empNo = User.Identity?.Name ?? "";

        // TEMP: replace later with DB lookup
        var vm = new EmployeeDetailsViewModel
        {
            EmployeeId = empNo,
            EmployeeName = "Mr. ARPIT CHOUREY (AM(F&S))"
        };

        return View(vm);
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult PermitDetails()
    {
        return View();
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult Violations()
    {
        return View();
    }
}
