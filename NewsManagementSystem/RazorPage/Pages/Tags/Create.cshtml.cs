using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.TagDTOs;

namespace RazorPage.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _service;

        public CreateModel(ITagService service)
        {
            _service = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public PostTagDTO Tag { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync([FromForm] PostTagDTO tag)
        {
            Tag = tag;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _service.CreateTag(Tag);

            return RedirectToPage($"./Index");
        }
    }
}
