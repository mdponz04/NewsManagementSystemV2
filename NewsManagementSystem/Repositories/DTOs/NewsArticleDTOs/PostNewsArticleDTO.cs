namespace Repositories.DTOs.NewsArticleDTOs
{
    public class PostNewsArticleDTO : BaseNewsArticleDTO
    {
        public short CreateByID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<int> SelectedTags { get; set; } = new List<int>();
    }

}
