using AutoMapper;
using Data.Entities;
using BusinessLogic.DTOs.TagDTOs;

namespace BusinessLogic.MappingProfile
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, GetTagDTO>().ReverseMap();
            CreateMap<Tag, PostTagDTO>().ReverseMap();
            CreateMap<Tag, PutTagDTO>().ReverseMap();
        }
    }
}
