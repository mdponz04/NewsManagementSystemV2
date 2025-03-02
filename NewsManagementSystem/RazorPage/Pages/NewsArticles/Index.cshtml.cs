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

        public List<GetNewsArticleDTO> NewsArticles { get; set; }

        public async Task OnGetAsync(string? searchString)
        {
            if(searchString != null)
            {
                NewsArticles = await _newsArticleService.GetNewsArticleBySearchString(searchString);
                return;
            }
            // Fetch paginated search categories
            NewsArticles = await _newsArticleService.GetAllNewsArticle();
        }

    }
}
