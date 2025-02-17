using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.NewsArticleDTOs;
using Repositories.PaggingItem;

namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        // GET: Tags
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {

            // Fetch paginated user accounts
            PaginatedList<GetNewsArticleDTO> newsArticles = await _newsArticleService.GetNewsArticles(pageNumber, pageSize, null, null, null);

            // Pass data to the view
            return View(newsArticles);
        }

        // GET: Search Tags
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {

            // Fetch paginated user accounts
            PaginatedList<GetNewsArticleDTO> newsArticlesSearch = await _newsArticleService.GetNewsArticles(pageNumber, pageSize, null, searchString, null);
            // Pass data to the view
            return View("Index", newsArticlesSearch);
        }
    }
}
