using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.PaggingItem;
using Repositories.DTOs.TagDTOs;

namespace NewsManagementSystem.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Tags
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {

            // Fetch paginated user accounts
            PaginatedList<GetTagDTO> tags = await _tagService.GetTags(pageNumber, pageSize, null, null, null);

            // Pass data to the view
            return View(tags);
        }

        // GET: Search Tags
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {

            // Fetch paginated user accounts
            PaginatedList<GetTagDTO> tagsSearch = await _tagService.GetTags(pageNumber, pageSize, null, searchString, null);

            if (tagsSearch == null)
            {
                tagsSearch = await _tagService.GetTags(pageNumber, pageSize, null, null, searchString);
            }

            // Pass data to the view
            return View("Index", tagsSearch);
        }
    }
}
