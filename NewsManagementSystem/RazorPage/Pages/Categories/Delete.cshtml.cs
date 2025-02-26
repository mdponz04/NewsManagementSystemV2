using AutoMapper;
using BusinessLogic.Interfaces;
using Data.DTOs.CategoryDTOs;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public GetCategoryDTO Category { get; set; }

        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Display delete confirmation page
        public async Task<IActionResult> OnGetAsync(short id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var categoryResult = await _categoryService.GetCategories(1, 1, id, null, null, null, null);
            if (categoryResult.Items.Count == 0)
            {
                return NotFound();
            }

            Category = categoryResult.Items.FirstOrDefault();
            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: Confirm deletion
        public async Task<IActionResult> OnPostAsync()
        {
            if (Category == null || Category.CategoryId <= 0)
            {
                return NotFound();
            }

            try
            {
                await _categoryService.DeleteCategory(Category.CategoryId);
                return RedirectToPage("Index"); // Redirect if deletion is successful
            }
            catch (ErrorException ex)
            {
                // Show error message in the UI
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page(); // Stay on the same page to show the popup

        }

    }
}
