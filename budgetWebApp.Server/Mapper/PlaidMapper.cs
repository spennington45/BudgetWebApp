using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class PlaidMapper : Profile
    {
        public PlaidMapper()
        {
            CreateMap<PlaidLinkRequestDto, PlaidItem>()
            .ForMember(dest => dest.PlaidItemId, opt => opt.Ignore())
            .ForMember(dest => dest.ItemId, opt => opt.Ignore())          // set after exchange
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())     // set after exchange
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.PlaidAccounts, opt => opt.Ignore())   // handled separately
            .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<PlaidAccountDto, PlaidAccount>()
                .ForMember(dest => dest.PlaidAccountId, opt => opt.Ignore())
                .ForMember(dest => dest.PlaidItemId, opt => opt.Ignore())     // set after item is saved
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.PlaidItem, opt => opt.Ignore());

        }
    }
}
