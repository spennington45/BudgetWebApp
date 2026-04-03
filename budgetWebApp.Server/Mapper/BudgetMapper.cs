using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class BudgetMapper : Profile
    {
        public BudgetMapper()
        {
            CreateMap<Budget, BudgetDto>().ReverseMap();
        }

    }
}
