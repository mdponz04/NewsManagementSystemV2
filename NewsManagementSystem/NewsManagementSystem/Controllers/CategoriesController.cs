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

        // GET: Categories
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {

            // Fetch paginated categories
            PaginatedList<GetCategoryDTO> categories = await _categoryService.GetCategories(pageNumber, pageSize, null, null, null, null, null);

            // Pass data to the view
            return View(categories);
        }

        // GET: Search Categories
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {

            // Fetch paginated search categories
            PaginatedList<GetCategoryDTO> categorySearch = await _categoryService.GetCategories(pageNumber, pageSize, null, searchString, null, null, null);


            // Pass data to the view
            return View("Index", categorySearch);
        }

        // GET: Create Category
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDTO categoryDto)
        {
            if (ModelState.IsValid)
            {
                // Create category
                var createdCategory = await _categoryService.CreateCategory(categoryDto);
                return RedirectToAction(nameof(Index)); // Redirect to index after successful creation
            }
            return View(categoryDto);
        }

        // GET: Edit Category
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Fetch category by id
            var category = await _categoryService.GetCategories(1, 1, id, null, null, null, null);
            if (category.Items.Count == 0)
            {
                return NotFound();
            }

            var categoryToUpdate = category.Items.FirstOrDefault();
            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            return View(categoryToUpdate);
        }

        // POST: Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryDTO categoryDto)
        {
            if (id <= 0 || categoryDto == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update category
                await _categoryService.UpdateCategory(id, categoryDto);
                return RedirectToAction(nameof(Index)); // Redirect to index after successful update
            }

            return View(categoryDto);
        }

        // GET: Delete Category
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Fetch category by id
            var category = await _categoryService.GetCategories(1, 1, id, null, null, null, null);
            if (category.Items.Count == 0)
            {
                return NotFound();
            }

            var categoryToDelete = category.Items.FirstOrDefault();
            if (categoryToDelete == null)
            {
                return NotFound();
            }

            return View(categoryToDelete);
        }

        // POST: Delete Category
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Delete category
            await _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(Index)); // Redirect to index after successful deletion
        }
    }


}

