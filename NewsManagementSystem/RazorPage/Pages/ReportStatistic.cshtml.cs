using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.NewsArticleDTOs;
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

        // Bindable properties to receive the filter dates from the query string.
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        // This list holds the grouped report data to be passed to the view.
        public List<NewsCreationReport> NewsCreationReports { get; set; } = new List<NewsCreationReport>();

        public async Task OnGetAsync()
        {
            // Retrieve all news articles using your service.
            List<GetNewsArticleDTO> newsArticles = await _newsArticleService.GetAllNewsArticle();

            // Apply the date filter if values are provided.
            if (FromDate.HasValue)
            {
                newsArticles = newsArticles
                    .Where(n => n.CreatedDate.HasValue &&
                           n.CreatedDate.Value.Date >= FromDate.Value.Date)
                    .ToList();
            }
            if (ToDate.HasValue)
            {
                newsArticles = newsArticles
                    .Where(n => n.CreatedDate.HasValue &&
                           n.CreatedDate.Value.Date <= ToDate.Value.Date)
                    .ToList();
            }

            // Group by the date the news article was created (ignoring the time part).
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
}
