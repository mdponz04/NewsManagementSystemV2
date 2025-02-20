namespace Data.DTOs.NewsArticleDTOs
{
    public class GetNewsArticleDTO : BaseNewsArticleDTO
    {
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public string NewsArticleId { get; set; } = null!;
        public short? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public short? UpdatedById { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
