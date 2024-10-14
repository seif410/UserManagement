using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private static string Success = null;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel user, IFormFile profilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            var result = await _accountService.RegisterAsync(user, profilePicture);
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
            var result = await _accountService.LoginAsync(user);
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
            _accountService.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var user = await _accountService.GetUser(User);
            ViewBag.Success = Success;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(AccountManageViewModel reqUser,
            IFormFile profilePicture)
        {
            ModelState.Remove("ProfilePicture");
            var user = await _accountService.GetUser(User);
            if (reqUser.ProfilePicture == null)
                reqUser.ProfilePicture = user.ProfilePicture;
            if (!ModelState.IsValid)
            {
                return View("Manage", reqUser);
            }
            var result = await _accountService.UpdateUser(User, reqUser,
                profilePicture);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error);
                return View("Manage", reqUser);
            }
            Success = "Profile data updated successfully.";
            return RedirectToAction();
        }
    }
}