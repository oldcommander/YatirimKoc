using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Email ve şifre zorunludur.");
            return View();
        }

        // Email üzerinden user bul
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError("", "Email veya şifre hatalı.");
            return View();
        }

        // Password ile login
        var result = await _signInManager.PasswordSignInAsync(
            user.UserName, // userName doğru
            password,
            isPersistent: true,
            lockoutOnFailure: false
        );
        if (!user.IsActive)
        {
            ModelState.AddModelError("", "Hesabınız aktif değil. Lütfen yöneticiyle iletişime geçin.");
            return View();
        }

        if (result.Succeeded)
        {
            // ✅ return ekledik
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        ModelState.AddModelError("", "Email veya şifre hatalı.");
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
