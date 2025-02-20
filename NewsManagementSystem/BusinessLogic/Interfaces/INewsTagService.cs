using Data.Entities;

namespace BusinessLogic.Interfaces
{
    public interface INewsTagService
    {
        Task<IEnumerable<NewsTag>> GetNewsTagListByArticleId(string articleId);
    }
}
