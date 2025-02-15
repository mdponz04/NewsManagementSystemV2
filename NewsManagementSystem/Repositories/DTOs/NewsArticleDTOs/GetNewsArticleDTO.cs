namespace Repositories.DTOs.NewsArticleDTOs
{
    public class GetNewsArticleDTO : BaseNewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;
        public short CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public short UpdatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
