using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using BusinessLogic.DTOs.NewsArticleDTOs;
using BusinessLogic.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IJwtTokenService _jwtTokenService;

        public EditModel(INewsArticleService newsArticleService
            , ITagService tagService
            , ICategoryService categoryService
            , IJwtTokenService jwtTokenService)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _categoryService = categoryService;
            _jwtTokenService = jwtTokenService;
        }
        public List<int> SelectedTagIds { get; set; } = default!;
        public List<GetTagDTO> Tags { get; set; } = default!;
        public IEnumerable<GetCategoryDTO> Category { get; set; } = default!;
        public GetNewsArticleDTO GetNewsArticleDTO { get; set; } = default!;
        [BindProperty]
        public PutNewsArticleDTO NewsArticle { get; set; } = default!;

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> OnGetAsync(string id)
        {
            Tags = await _tagService.GetAllTag();
            Category = await _categoryService.GetAllCategories();
            GetNewsArticleDTO = await _newsArticleService.GetNewsArticleById(id);
            SelectedTagIds = await _newsArticleService.GetTagIdsByNewsArticleId(id);

            NewsArticle = new();
            NewsArticle.NewsArticleId = id;
            NewsArticle.NewsStatus = GetNewsArticleDTO.NewsStatus;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> OnPostAsync()
        {
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            NewsArticle.UpdatedById = short.Parse(_jwtTokenService.GetId(jwtTokenFromSession));

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return NotFound("Invalid model state!");
            }
            await _newsArticleService.UpdateNewsArticle(NewsArticle);

            return RedirectToPage("./Index");
        }
    }
}
