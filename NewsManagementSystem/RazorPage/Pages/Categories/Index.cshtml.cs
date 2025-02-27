using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public PaginatedList<GetCategoryDTO> Categories { get; set; }
        public IndexModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        // GET: Get and Search Categories
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {
            // Fetch paginated search categories
            Categories = await _categoryService.GetCategories(pageNumber, pageSize, null, searchString, null, null, null);
        }

    }
}
