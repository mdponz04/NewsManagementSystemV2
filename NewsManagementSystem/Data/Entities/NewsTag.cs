namespace Data.Entities
{
    public partial class NewsTag
    {
        public string NewsArticleId { get; set; } = null!;
        public int TagId { get; set; }
        public virtual NewsArticle? NewsArticle { get; set; } = null!;
        public virtual Tag? Tag { get; set; } = null!;
    }
}