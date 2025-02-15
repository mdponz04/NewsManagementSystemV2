namespace Repositories.DTOs.NewsArticleDTOs
{
    public class PutNewsArticleDTO : BaseNewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;
        public short UpdateByID { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
