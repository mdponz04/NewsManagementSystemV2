using AutoMapper;
using Data.Entities;
using Data.DTOs.NewsArticleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.MappingProfile
{
    public class NewsArticleProfile : Profile
    {
        public NewsArticleProfile()
        {
            CreateMap<NewsArticle, GetNewsArticleDTO>().ReverseMap();
            CreateMap<NewsArticle, PostNewsArticleDTO>().ReverseMap();
            CreateMap<NewsArticle, PutNewsArticleDTO>().ReverseMap();
        }
    }
}
