﻿using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.ViewModels;
using UserManagement.Services;

namespace UserManagement.Controllers
{
	public class RoleController : Controller
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var roles = _roleService.GetRoles();
			return View(roles);
		}

		[HttpGet]
		public IActionResult AddRole() { return View(); }

		[HttpPost]
		public async Task<IActionResult> AddRole(RoleVM role)
		{
			if (!ModelState.IsValid)
				return View("AddRole", role);
			var result = await _roleService.AddRoleAsync(role);
			if (!result.Success)
			{
				ModelState.AddModelError("", result.Error);
				return View("AddRole", role);
			}
			return Json("Created Successfully");
		}
	}
}