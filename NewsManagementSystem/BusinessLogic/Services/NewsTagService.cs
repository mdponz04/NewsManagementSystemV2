using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;

namespace BusinessLogic.Services
{
    public class NewsTagService : INewsTagService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;

        public NewsTagService(IUOW unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NewsTag>> GetNewsTagListByArticleId(string articleId)
        {
            IQueryable<NewsTag> query = _unitOfWork
                .GetRepository<NewsTag>()
                .Entities
                .Where(x => x.NewsArticleId == articleId);
            IEnumerable<NewsTag> newsTagsList = query.ToList();
            return newsTagsList;
        }
    }
}
