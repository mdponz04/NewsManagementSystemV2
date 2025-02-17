using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.CategoryDTOs;
using Repositories.PaggingItem;

namespace NewsManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Tags
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {

            // Fetch paginated categories
            PaginatedList<GetCategoryDTO> categories = await _categoryService.GetCategories(pageNumber, pageSize, null, null, null, null, null);

            // Pass data to the view
            return View(categories);
        }

        // GET: Search Tags
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {

            // Fetch paginated search categories
            PaginatedList<GetCategoryDTO> categorySearch = await _categoryService.GetCategories(pageNumber, pageSize, null, searchString, null, null, null);


            // Pass data to the view
            return View("Index", categorySearch);
        }
    }
}
