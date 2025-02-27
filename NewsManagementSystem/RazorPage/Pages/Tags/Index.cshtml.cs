using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public PaginatedList<GetTagDTO> Tags { get; set; }
        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Get and Search Tag
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {
            // Fetch paginated search tags
            Tags = await _tagService.GetTags(pageNumber, pageSize, null, searchString, null);
        }
    }
}
