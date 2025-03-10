﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using BusinessLogic.DTOs.NewsArticleDTOs;
using BusinessLogic.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IJwtTokenService _jwtTokenService;

        public CreateModel(INewsArticleService newsArticleService
            , ITagService tagService
            , ICategoryService categoryService
            , IJwtTokenService jwtTokenService)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _categoryService = categoryService;
            _jwtTokenService = jwtTokenService;
        }
        public List<GetTagDTO> Tags { get; set; } = default!;
        public IEnumerable<GetCategoryDTO> Category { get; set; } = default!;
        [BindProperty]
        public PostNewsArticleDTO NewsArticle { get; set; } = default!;
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnGetAsync()
        {
            Tags = await _tagService.GetAllTag();
            Category = await _categoryService.GetAllCategories();
            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnPostAsync()
        {
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            NewsArticle.CreatedById = short.Parse(_jwtTokenService.GetId(jwtTokenFromSession));

            if (!ModelState.IsValid)
            {
                Tags = await _tagService.GetAllTag();
                Category = await _categoryService.GetAllCategories();
                return Page();
            }
            await _newsArticleService.CreateNewsArticle(NewsArticle);

            return RedirectToPage("./Index");
        }
    }
}
