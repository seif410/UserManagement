using AutoMapper;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagement.Models.ViewModels;

namespace UserManagement.Mapping
{
	public class Mapper : Profile
	{
		public Mapper()
		{
			CreateMap<RegisterViewModel, ApplicationUser>().ReverseMap();
			CreateMap<AccountManageViewModel, ApplicationUser>().ReverseMap();
			CreateMap<RoleViewModel, IdentityRole>().ReverseMap();
		}
	}
}