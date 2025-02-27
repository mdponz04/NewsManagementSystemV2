using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.NewsArticleDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly INewsTagService _newsTagService;
        private readonly IJwtTokenService _jwtTokenService;
        public IndexModel(
            ICategoryService categoryService
            , ITagService tagService
            , INewsArticleService newsArticleService
            , INewsTagService newsTagService
            , IJwtTokenService jwtTokenService)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _newsTagService = newsTagService;
            _jwtTokenService = jwtTokenService;
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
