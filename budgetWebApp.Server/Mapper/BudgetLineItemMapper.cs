using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class BudgetLineItemMapper : Profile
    {
        public BudgetLineItemMapper()
        {
            CreateMap<BudgetLineItem, BudgetLineItemDto>().ReverseMap();
        }
    }
}
