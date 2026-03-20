using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
