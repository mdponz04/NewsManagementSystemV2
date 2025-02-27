using BusinessLogic.Interfaces;
using Data.DTOs.NewsArticleDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public PaginatedList<GetNewsArticleDTO> NewsArticles { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {
            if(searchString != null)
            {
                searchString = searchString.Trim();
            }
            // Fetch paginated search categories
            NewsArticles = await _newsArticleService.GetNewsArticles(pageNumber, pageSize, null, searchString, null);
        }

    }
}
