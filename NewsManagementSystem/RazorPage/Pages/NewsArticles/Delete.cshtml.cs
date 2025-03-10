﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.NewsArticleDTOs;
using BusinessLogic.DTOs.CategoryDTOs;
using BusinessLogic.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public DeleteModel(INewsArticleService newsArticleService, ITagService tagService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public GetNewsArticleDTO NewsArticle { get; set; } = default!;
        public GetCategoryDTO Category { get; set; } = default!;
        public List<GetTagDTO> Tags { get; set; } = default!;
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NewsArticle = await _newsArticleService.GetNewsArticleById(id);
            Category = await _categoryService.GetCategoryById(NewsArticle.CategoryId);
            Tags = await _tagService.GetListTag(await _newsArticleService.GetTagIdsByNewsArticleId(id));
            return Page();
        }
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _newsArticleService.DeleteNewsArticle(id);
            return RedirectToPage("./Index");
        }
    }
}
