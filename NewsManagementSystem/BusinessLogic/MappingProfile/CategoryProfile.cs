using AutoMapper;
using Data.Entities;
using Repositories.DTOs.CategoryDTOs;
using Repositories.DTOs.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.MappingProfile
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoryDTO>().ReverseMap();
        }
    }
}
