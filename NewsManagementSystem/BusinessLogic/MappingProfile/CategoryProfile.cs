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
            CreateMap<Category, GetCategoryDTO>()
                .ReverseMap();

            CreateMap<Category, CreateCategoryDTO>()
                // When mapping from Category to CreateCategoryDTO, map CategoryDesciption to CategoryDescription.
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.CategoryDesciption))
                .ReverseMap()
                // When mapping from CreateCategoryDTO back to Category, map CategoryDescription to CategoryDesciption.
                .ForMember(dest => dest.CategoryDesciption, opt => opt.MapFrom(src => src.CategoryDescription));

            CreateMap<Category, UpdateCategoryDTO>()
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.CategoryDesciption))
                .ReverseMap()
                .ForMember(dest => dest.CategoryDesciption, opt => opt.MapFrom(src => src.CategoryDescription))    
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore());

            CreateMap<GetCategoryDTO, UpdateCategoryDTO>();

        }
    }
}
