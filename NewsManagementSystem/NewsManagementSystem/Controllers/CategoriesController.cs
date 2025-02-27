using AutoMapper;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.DTOs.CategoryDTOs;
using Data.PaggingItem;

namespace NewsManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO categoryDto)
        {
            if (ModelState.IsValid)
            {
                var createdCategory = await _categoryService.CreateCategory(categoryDto);

                // If this is an AJAX request, return JSON rather than a redirect.
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is invalid and the request is AJAX, send a JSON error response.
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, error = "Validation failed" });
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

            var updateCategoryDto = _mapper.Map<UpdateCategoryDTO>(getCategoryDto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditCategoryPartial", updateCategoryDto);
            }

            return View(updateCategoryDto);
        }

        // POST: Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromBody] UpdateCategoryDTO categoryDto)
        {
            if (id <= 0 || categoryDto == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update category using the service
                await _categoryService.UpdateCategory(id, categoryDto);

                // Return JSON response if AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, return JSON error response for AJAX requests
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, error = "Validation failed" });
            }
            return View(categoryDto);
        }

        // GET: Delete Category
        public async Task<IActionResult> Delete(short id)
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
        public async Task<IActionResult> DeleteConfirmed(short id)
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

