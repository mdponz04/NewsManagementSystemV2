using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.CategoryDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.Categories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public PaginatedList<GetCategoryDTO> Categories { get; set; }
        public IndexModel(ICategoryService categoryService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }


        // GET: Get and Search Categories
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            var userRole = _jwtTokenService.GetRole(jwtToken);
            ViewData["JwtToken"] = jwtToken;
            ViewData["UserRole"] = userRole;
            Console.WriteLine($"JWT Token: {jwtToken}");
            // Fetch paginated search categories
            Categories = await _categoryService.GetCategories(pageNumber, pageSize, null, searchString, null, null, null);
        }

    }
}
