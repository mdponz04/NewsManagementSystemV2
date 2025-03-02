using Data.DTOs.NewsArticleDTOs;
using Data.PaggingItem;
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
        Task<List<GetNewsArticleDTO>> GetNewsArticleBySearchString(string search);
        Task<List<int>> GetTagIdsByNewsArticleId(string newsArticleId);
        Task<PaginatedList<GetNewsArticleDTO>> GetNewsArticles(int index, int pageSize, string? idSearch, string? titleSearch, string? headlineSearch);
    }
}
