using Repositories.DTOs.NewsArticleDTOs;

namespace BusinessLogic.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<GetNewsArticleDTO>> GetAllNewsArticle();
        Task<GetNewsArticleDTO> GetNewsArticleById(string id);
        Task<string> CreateNewsArticle(PostNewsArticleDTO newsArticle);
        Task UpdateNewsArticle(PutNewsArticleDTO updatedNewsArticle);
        Task DeleteNewsArticle(string id);
        Task<List<GetNewsArticleDTO>> GetActiveNewsArticle();
        Task<List<GetNewsArticleDTO>> GetNewsArticleAccordingToCreateById(short createById);
    }
}
