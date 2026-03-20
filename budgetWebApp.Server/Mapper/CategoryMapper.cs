using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
