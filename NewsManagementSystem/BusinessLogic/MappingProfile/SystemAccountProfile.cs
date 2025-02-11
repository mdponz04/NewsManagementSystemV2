using AutoMapper;
using Data.Entities;
using Repositories.DTOs.SystemAccountDTOs;

namespace BusinessLogic.MappingProfile
{
    public class SystemAccountProfile : Profile
    {
        public SystemAccountProfile()
        {
            CreateMap<SystemAccount, GetSystemAccountDTO>().ReverseMap();
            CreateMap<SystemAccount, PostSystemAccountDTO>().ReverseMap();
            CreateMap<SystemAccount, PutSystemAccountDTO>().ReverseMap();
        }
    }
}
