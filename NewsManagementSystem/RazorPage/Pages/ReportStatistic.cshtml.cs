using BusinessLogic.Interfaces;
using Data.DTOs.NewsArticleDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class ReportStatisticModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportStatisticModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        // This list will hold the grouped report data
        public List<NewsCreationReport> NewsCreationReports { get; set; } = new List<NewsCreationReport>();

        public async Task OnGetAsync()
        {
            // Retrieve all news articles from the service
            List<GetNewsArticleDTO> newsArticles = await _newsArticleService.GetAllNewsArticle();

            // Group news articles by the date they were created and count them
            NewsCreationReports = newsArticles
                .Where(n => n.CreatedDate.HasValue)
                .GroupBy(n => n.CreatedDate.Value.Date)
                .Select(g => new NewsCreationReport
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(report => report.Date)
                .ToList();
        }
    }

    // A simple DTO to hold the report data
    public class NewsCreationReport
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
