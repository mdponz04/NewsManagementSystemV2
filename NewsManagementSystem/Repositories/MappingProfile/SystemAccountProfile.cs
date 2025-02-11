using AutoMapper;
using Data.Entities;
using Repositories.DTOs.SystemAccountDTOs;

namespace Repositories.MappingProfile
{
    public class SystemAccountProfile : Profile
    {
        public SystemAccountProfile() 
        {
            CreateMap<SystemAccount, GetSystemAccountDTO>().ReverseMap();
        }
    }
}
