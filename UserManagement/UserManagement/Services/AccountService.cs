using AutoMapper;
using Azure.Core;
using EFCore.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountService(UserManager<ApplicationUser> userManager,
            IMapper mapper, SignInManager<ApplicationUser> signInManager,
            IRoleService roleService, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleService = roleService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel user, IFormFile profilePic)
        {
            if (await _userManager.FindByEmailAsync(user.Email) is not null)
                return new AuthResult
                {
                    Error = "This email address is already registered. " +
                    "Please user a different email or log in"
                };
            if (profilePic != null)
            {
                using (var datastream = new MemoryStream())
                {
                    await profilePic.CopyToAsync(datastream);
                    user.ProfilePicture = datastream.ToArray();
                }
            }
            else
            {
                var imgFile = Path.Combine(_webHostEnvironment.WebRootPath,
                    "Images", "1.jpg");
                user.ProfilePicture = await System.IO.File
                    .ReadAllBytesAsync(imgFile);
            }
            var appUser = _mapper.Map<ApplicationUser>(user);
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (!result.Succeeded)
            {
                var errorMsg = string.Empty;
                foreach (var error in result.Errors)
                    errorMsg += $"{error.Description}. \n";
                return new AuthResult { Error = errorMsg };
            }
            try
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            }
            catch (Exception ex)
            {
                await _roleService.AddRoleAsync(new RoleViewModel
                {
                    Name = "User"
                });
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            }
            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser is null || !await _userManager.CheckPasswordAsync(appUser, user.Password))
                return new AuthResult { Error = "Invalid email or password. Please try again." };
            await _signInManager.SignInAsync(appUser, user.RememberMe);
            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> UpdateUser(ClaimsPrincipal user,
            AccountManageViewModel reqUser, IFormFile profilePic)
        {
            var appUser = await _userManager.GetUserAsync(user);
            IdentityResult result = new();
            if (reqUser.UserName != appUser.UserName)
            {
                appUser.UserName = reqUser.UserName;
                result = await _userManager.UpdateAsync(appUser);

            }
            if (reqUser.FirstName != appUser.FirstName)
            {
                appUser.FirstName = reqUser.FirstName;
                result = await _userManager.UpdateAsync(appUser);
            }
            if (reqUser.LastName != appUser.LastName)
            {
                appUser.LastName = reqUser.LastName;
                result = await _userManager.UpdateAsync(appUser);
            }
            if (profilePic is not null)
            {
                using (var datastream = new MemoryStream())
                {
                    await profilePic.CopyToAsync(datastream);
                    appUser.ProfilePicture = datastream.ToArray();
                }
                result = await _userManager.UpdateAsync(appUser);
            }

            if (!result.Succeeded)
            {
                var errorMsg = string.Empty;
                foreach (var error in result.Errors)
                    errorMsg += $"{error.Description} \n";
                return new AuthResult
                {
                    Error = errorMsg
                };
            }
            await _signInManager.RefreshSignInAsync(appUser);
            return new AuthResult { Success = true };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AccountManageViewModel> GetUser(ClaimsPrincipal user)
        {
            return _mapper.Map<AccountManageViewModel>(await _userManager.
                GetUserAsync(user));
        }
    }
}