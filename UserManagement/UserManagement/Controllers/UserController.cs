using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var userRoles = await _userService.GetUserRolesAsync();
            return View(userRoles);
        }
    }
}
