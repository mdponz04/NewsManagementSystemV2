using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RazorPage.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;


        [BindProperty]
        public CreateCategoryDTO CategoryDto { get; set; }

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Create Category
        public IActionResult OnGet()
        {
            return Page();
        }


        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the request is JSON (AJAX)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    CategoryDto = JsonSerializer.Deserialize<CreateCategoryDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }

            // Validate data
            if (!ModelState.IsValid || CategoryDto == null)
            {
                return new JsonResult(new { success = false, error = "Validation failed" });
            }

            // Call service to create category
            var createdCategory = await _categoryService.CreateCategory(CategoryDto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true });
            }

            return RedirectToPage("/Categories/Index");
        }


    }
}
