using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class RecurringExpenseMapper : Profile
    {
        public RecurringExpenseMapper()
        {
            CreateMap<RecurringExpense, RecurringExpenseDto>().ReverseMap();
        }
    }
}
