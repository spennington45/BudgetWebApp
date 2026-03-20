using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class SourceTypeMapper : Profile
    {
        public SourceTypeMapper()
        {
            CreateMap<SourceType, SourceTypeDto>().ReverseMap();
        }
    }
}
