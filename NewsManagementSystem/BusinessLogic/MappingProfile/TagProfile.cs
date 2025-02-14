using Data.Entities;
using Repositories.DTOs.TagDTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.MappingProfile
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, GetTagDTO>().ReverseMap();
        }
        
    }
}
