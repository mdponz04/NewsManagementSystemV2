using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly ITagService _tagService;

        public DetailsModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public GetTagDTO Tag { get; set; } = default!;
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tag = await _tagService.GetTagById(id);
            return Page();
        }
    }
}
