using Microsoft.AspNetCore.Mvc;
using Data.Entities;
using BusinessLogic.Interfaces;
using Repositories.DTOs.TagDTOs;

namespace NewsManagementSystem.Controllers
{
    public class TagsController : Controller
    {
        private readonly NewsManagementDbContext _context;
        private readonly ITagService _tagService;

        public TagsController(NewsManagementDbContext context, ITagService tagService)
        {
            _context = context;
            _tagService = tagService;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTag();
            return View(await _tagService.GetAllTag());
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GetTagDTO tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostTagDTO tag)
        {
            
            int id = await _tagService.CreateTag(tag);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            GetTagDTO tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PutTagDTO tag)
        {
            if (id != tag.TagId)
            {
                return NotFound();
            }

            await _tagService.UpdateTag(tag);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            GetTagDTO tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tagService.DeleteTag(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.TagId == id);
        }
    }
}
