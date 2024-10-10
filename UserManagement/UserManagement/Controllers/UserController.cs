using EFCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userRoles = await _userService.GetUsersRolesAsync();
            return View(userRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userId)
        {
            var userRoles = await _userService.GetUserRolesAsync(userId);
            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(SelectedRolesViewModel selectedRoles)
        {
            await _userService.SaveRoleChanges(selectedRoles);
            return RedirectToAction("Index");
        }

    }
}
