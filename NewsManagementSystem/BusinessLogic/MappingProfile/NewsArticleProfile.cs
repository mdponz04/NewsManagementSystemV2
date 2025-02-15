using AutoMapper;
using Data.Entities;
using Repositories.DTOs.NewsArticleDTOs;

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
