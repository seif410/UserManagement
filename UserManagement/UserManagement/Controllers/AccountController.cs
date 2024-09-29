using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
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
		public async Task<IActionResult> Register(RegisterVM user)
		{
			if (!ModelState.IsValid)
				return View("Register", user);
			var result = await accountService.RegisterAsync(user);
			if (!result.Success)
			{
				ModelState.AddModelError("", result.Error);
				return View("Register", user);
			}
			return Json("test");
		}
	}
}