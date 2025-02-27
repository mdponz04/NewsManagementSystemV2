namespace BusinessLogic.DTOs.NewsArticleDTOs
{
    public class PutNewsArticleDTO : BaseNewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;
        public short UpdatedById { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public List<int> SelectedTags { get; set; } = new List<int>();
    }
}
