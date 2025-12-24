using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PemitManagement.Data;
using PemitManagement.Identity;
using PemitManagement.Models;
using PemitManagement.Services;
using PemitManagement.ViewModels;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PermissionClaimService _permissionClaimService;


    public AccountController(
        ApplicationDbContext db,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        PermissionClaimService permissionClaimService)
    {
        _db = db;
        _signInManager = signInManager;
        _userManager = userManager;
        _permissionClaimService = permissionClaimService;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var employee = await _db.Employees
            .FirstOrDefaultAsync(e => e.EmpNo == model.EmpNo && e.Active == true);

        if (employee == null || employee.Password != model.Password)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.EmpNo);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = model.EmpNo,
                EmployeeId = employee.Id
            };
            await _userManager.CreateAsync(user);
        }
        await _permissionClaimService.SyncPermissionsAsync(user);

        var permissions = await _db.UserPermissions
            .Where(up => up.UserId == employee.Id)
            .Select(up => up.Permission.Name)
            .ToListAsync();

        var claims = permissions
            .Select(p => new Claim("permission", p))
            .ToList();

        await _signInManager.SignInWithClaimsAsync(
            user,
            isPersistent: false,
            claims
        );

        await _signInManager.SignInAsync(user, false);

        TempData["ToastMessage"] = "Logged in successfully";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["ToastMessage"] = "Logged out successfully";
        return RedirectToAction("Login", "Account");
    }

    public IActionResult Profile()
    {
        return View();
    }
}

