using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel user, IFormFile ProfilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if (!ModelState.IsValid || ProfilePicture == null)
            {
                ModelState.AddModelError("ProfilePicture", "Profile picture field is required");
                return View("Register", user);
            }
            var imgFile = Request.Form.Files.FirstOrDefault();
            using (var datastream = new MemoryStream())
            {
                await imgFile.CopyToAsync(datastream);
                user.ProfilePicture = datastream.ToArray();
            }
            var result = await accountService.RegisterAsync(user);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error!);
                return View("Register", user);
            }
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);
            var result = await accountService.LoginAsync(user);
            if (!result.Success)
            {
                var errorMsg = result.Error!;
                ModelState.AddModelError("", errorMsg);
                return View("Login", user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            accountService.Logout();
            return RedirectToAction("Login");
        }
    }
}