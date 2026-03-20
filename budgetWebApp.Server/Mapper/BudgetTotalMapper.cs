using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class BudgetTotalMapper : Profile
    {
        public BudgetTotalMapper()
        {
            CreateMap<BudgetTotal, BudgetTotalDto>().ReverseMap();
        }
    }
}
