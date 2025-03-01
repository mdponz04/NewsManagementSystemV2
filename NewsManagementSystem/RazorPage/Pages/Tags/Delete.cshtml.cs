using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;

namespace RazorPage.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public GetTagDTO Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tag = await _tagService.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }
            else
            {
                Tag = tag;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _tagService.DeleteTag(id);
            return RedirectToPage("./Index");
        }
    }
}
