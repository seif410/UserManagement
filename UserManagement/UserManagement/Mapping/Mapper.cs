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
			CreateMap<RegisterVM, ApplicationUser>().ReverseMap();
			CreateMap<RoleVM, IdentityRole>().ReverseMap();
		}
	}
}