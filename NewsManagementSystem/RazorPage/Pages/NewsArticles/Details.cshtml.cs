using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.NewsArticleDTOs;
using Data.DTOs.TagDTOs;
using Data.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.NewsArticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public DetailsModel(INewsArticleService newsArticleService, ITagService tagService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public GetNewsArticleDTO NewsArticle { get; set; } = default!;
        public List<GetTagDTO> Tags { get; set; } = default!;
        public GetCategoryDTO Category { get; set; } = default!;
        [AllowAnonymous]
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NewsArticle = await _newsArticleService.GetNewsArticleById(id);
            Tags = await _tagService.GetListTag(
                await _newsArticleService.GetTagIdsByNewsArticleId(id));
            Category = await _categoryService.GetCategoryById(NewsArticle.CategoryId);
            return Page();
        }
    }
}
