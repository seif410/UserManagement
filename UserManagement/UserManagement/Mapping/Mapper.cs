using AutoMapper;
using EFCore.Entities;
using UserManagement.Models.ViewModels;

namespace UserManagement.Mapping
{
	public class Mapper : Profile
	{
		public Mapper()
		{
			CreateMap<RegisterVM, ApplicationUser>().ReverseMap();
		}
	}
}