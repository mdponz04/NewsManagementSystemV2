using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly ITagService _tagService;

        public EditModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public GetTagDTO Tag { get; set; } = default!;
        [BindProperty]
        public PutTagDTO UpdatedTag { get; set; } = default!;
        [Authorize(Roles = "1")]
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tag = await _tagService.GetTagById(id);
            return Page();
        }
        [Authorize(Roles = "1")]
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if(UpdatedTag == null)
            {
                return NotFound("Updated tag not found!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _tagService.UpdateTag(UpdatedTag);
            return RedirectToPage("./Index");
        }
    }
}
