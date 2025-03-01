using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public List<GetTagDTO> Tags { get; set; }
        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Get and Search Tag
        public async Task OnGetAsync()
        {
            // Fetch paginated search tags
            Tags = await _tagService.GetAllTag();
        }
    }
}
