using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RazorPage.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        [BindProperty]
        public UpdateCategoryDTO CategoryDto { get; set; }

        public EditModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: Edit Category
        public async Task<IActionResult> OnGetAsync(int id)
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

            var getCategoryDto = categoryResult.Items.FirstOrDefault();
            if (getCategoryDto == null)
            {
                return NotFound();
            }

            CategoryDto = _mapper.Map<UpdateCategoryDTO>(getCategoryDto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Partial("_EditCategoryPartial", CategoryDto);
            }

            return Page();
        }

        // POST: Update Category
        public async Task<IActionResult> OnPostAsync([FromBody] UpdateCategoryDTO categoryDto)
        {
            if (!ModelState.IsValid || categoryDto == null)
            {
                return new JsonResult(new { success = false, error = "Validation failed" });
            }

            try
            {
                var updateResult = await _categoryService.UpdateCategory(categoryDto.CategoryID, categoryDto);
                if (updateResult == null)  // Check if update was successful
                {
                    return new JsonResult(new { success = false, error = "Update failed" });
                }

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }


    }
}

