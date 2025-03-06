using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.TagDTOs;
using Microsoft.AspNetCore.Authorization;

namespace RazorPage.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _service;

        public CreateModel(ITagService service)
        {
            _service = service;
        }
        [Authorize(Roles ="1")]
        public IActionResult OnGet()
        {
            return Page();
        }
        public PostTagDTO Tag { get; set; } = default!;
        [Authorize(Roles = "1")]
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
