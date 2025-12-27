using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PemitManagement.Data;
using PemitManagement.ViewModels.Observations;

[Authorize]
public class ObservationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ObservationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Policy = "create_observations")]
    public async Task<IActionResult> Create()
    {
        var empNo = User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(empNo))
            return Forbid();

        var employee = await _context.Employees
            .AsNoTracking()
            .Where(e => e.EmpNo == empNo)
            .Select(e => new EmployeeDetailsViewModel
            {
                EmployeeId = e.EmpNo,
                EmployeeName = string.IsNullOrWhiteSpace(e.Designation)
                    ? e.Name
                    : $"{e.Name} ({e.Designation})"
            })
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            TempData["toast-error"] = "Employee record not found.";
            return RedirectToAction("Index", "Home");
        }

        return View(employee);
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult PermitDetails()
    {
        var model = HttpContext.Session.GetObject<PermitDetailsViewModel>("permit-details")
                    ?? new PermitDetailsViewModel();

        return View(model);
    }

    [Authorize(Policy = "create_observations")]
    [HttpPost]
    public async Task<IActionResult> FetchPermit(PermitDetailsViewModel model)
    {
        model.HasSearched = true;

        if (!ModelState.IsValid)
        {
            TempData["toast-error"] = "Permit number is required.";
            return View("PermitDetails", model);
        }

        var permit = await _context.Permits
            .Where(p => p.PermitNumber == model.PermitNumber)
            .Select(p => new
            {
                Permit = p,
                PermitType = p.PermitType.Name,
                LocationName = p.Location.Name,
                Refinery = p.Location.RefineryType
            })
            .FirstOrDefaultAsync();

        if (permit == null)
        {
            model.PermitFound = false;
            TempData["toast-error"] = "Permit not found.";
            return View("PermitDetails", model);
        }

        // Fill model
        model.PermitFound = true;
        model.PermitType = permit.PermitType;
        model.AgencyName = permit.Permit.AgencyName;
        model.WorkOrderNumber = permit.Permit.WorkOrderNumber;
        model.ContractWorkerName = permit.Permit.ContractWorkerName;
        model.VisitedSite = permit.LocationName;
        model.Refinery = permit.Refinery;
        model.ExactLocation = permit.Permit.ExactLocation;
        model.ReceiverName = permit.Permit.ReceiverName;
        model.ReceiverEmployeeId = permit.Permit.ReceiverEmployeeId;

        // Issuers
        model.Issuers = await (
            from pi in _context.PermitIssuers
            join ir in _context.IssuerRoles on pi.IssuerRoleId equals ir.Id
            where pi.PermitId == permit.Permit.Id
            select new IssuerInfoViewModel
            {
                RoleName = ir.RoleName,
                EmployeeId = pi.EmployeeId,
                EmployeeName = pi.EmployeeName
            }
        ).ToListAsync();

        HttpContext.Session.SetObject("permit-details", model);
        return View("PermitDetails", model);
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult ClearPermitDetails()
    {
        HttpContext.Session.Remove("permit-details");
        return RedirectToAction(nameof(PermitDetails));
    }

    [Authorize(Policy = "create_observations")]
    public IActionResult Violations()
    {
        return View();
    }
}
