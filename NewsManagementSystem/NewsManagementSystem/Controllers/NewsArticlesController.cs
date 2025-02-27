using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.NewsArticleDTOs;
using BusinessLogic.DTOs.TagDTOs;
using Data.Entities;
using BusinessLogic.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;


namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly INewsTagService _newsTagService;
        private readonly IJwtTokenService _jwtTokenService;
        public NewsArticlesController(
            ICategoryService categoryService
            , ITagService tagService
            , INewsArticleService newsArticleService
            , INewsTagService newsTagService
            , IJwtTokenService jwtTokenService)
        {
            _categoryService = categoryService;
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _newsTagService = newsTagService;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        // GET: NewsArticles
        public async Task<IActionResult> Index(string? searchQuery)
        {
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");

            // Retrieve claims using the JWT token service
            string userRole = _jwtTokenService.GetRole(jwtTokenFromSession!);

            List<GetNewsArticleDTO> newsArticles;
            if (userRole == "0" || userRole == "1") 
            {
                newsArticles = await _newsArticleService.GetAllNewsArticle();
                return View(newsArticles);
            }
            else
            {
                newsArticles = await _newsArticleService.GetActiveNewsArticle();
            }

            // Apply search filter if query is provided
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                newsArticles = await _newsArticleService.GetNewsArticleBySearchString(searchQuery);
            }
            
            return View(newsArticles);
        }
        [Authorize]
        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var newsArticle = await _newsArticleService.GetNewsArticleById(id);

            IEnumerable<NewsTag> newsTagList = await _newsTagService.GetNewsTagListByArticleId(id);

            List<GetTagDTO> tagList = new List<GetTagDTO>();

            foreach(NewsTag newsTag in newsTagList)
            {
                GetTagDTO tag = await _tagService.GetTagById(newsTag.TagId);
                tagList.Add(tag);
            }

            GetCategoryDTO category = await _categoryService.GetCategoryById(newsArticle.CategoryId);
            
            if (newsArticle == null || tagList == null || category == null)
            {
                return NotFound();
            }

            ViewData["categoryName"] = category.CategoryName;
            ViewData["tagList"] = tagList;
            return View(newsArticle);
        }
        [Authorize(Roles = "0, 1, 2")]
        // GET: NewsArticles/Create
        public async Task<IActionResult> Create()
        {
            // Populating ViewData for Category and Tags
            ViewData["Tags"] = await _tagService.GetAllTag();
            ViewData["Categories"] = new SelectList(
                    await _categoryService.GetAllCategories()
                    , "CategoryId", "CategoryName");

            // Initialize an empty model to pass to the view
            var model = new PostNewsArticleDTO();
            return View(model);
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Create(PostNewsArticleDTO newsArticle)
        {
            if(!ModelState.IsValid)
            {
                ViewData["Tags"] = await _tagService.GetAllTag();
                ViewData["Categories"] = new SelectList(
                    await _categoryService.GetAllCategories()
                    , "CategoryId", "CategoryName");
                return View(newsArticle);
            }
            var tags = await _tagService.GetListTagByIdEntityType(newsArticle.SelectedTags);

            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            string userId = _jwtTokenService.GetId(jwtTokenFromSession!);
            newsArticle.CreatedById = short.Parse(userId);
            string id = await _newsArticleService.CreateNewsArticle(newsArticle);

            return RedirectToAction(nameof(Details), new { id });
        }
        [Authorize(Roles = "0, 1, 2")]
        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleById(id);

            IEnumerable<NewsTag> newsTagList = await _newsTagService.GetNewsTagListByArticleId(id);

            List<GetTagDTO> selectedTagList = new List<GetTagDTO>();

            foreach (NewsTag newsTag in newsTagList)
            {
                GetTagDTO tag = await _tagService.GetTagById(newsTag.TagId);
                selectedTagList.Add(tag);
            }

            if (newsArticle == null || selectedTagList == null)
            {
                return NotFound();
            }
            
            ViewData["AssignTags"] = selectedTagList;
            ViewData["Tags"] = await _tagService.GetAllTag();
            ViewData["Categories"] = new SelectList(
                    await _categoryService.GetAllCategories()
                    , "CategoryId", "CategoryName");
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName");
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Edit(PutNewsArticleDTO newsArticle)
        {
            ViewData["Tags"] = await _tagService.GetAllTag();
            ViewData["Categories"] = new SelectList(
                    await _categoryService.GetAllCategories()
                    , "CategoryId", "CategoryName");
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            if (newsArticle.NewsArticleId == null)
            {
                return NotFound();
            }

            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            string userId = _jwtTokenService.GetId(jwtTokenFromSession!);
            newsArticle.UpdatedById = short.Parse(userId);

            await _newsArticleService.UpdateNewsArticle(newsArticle);
            return RedirectToAction(nameof(Details), new { id = newsArticle.NewsArticleId });
        }

        // GET: NewsArticles/Delete/5
        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0, 1, 2")]
        public async Task<IActionResult> DeleteConfirmed(string NewsArticleId)
        {
            await _newsArticleService.DeleteNewsArticle(NewsArticleId);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "1")]
        // GET: CreatedNewsArticleHistory
        public async Task<IActionResult> CreatedNewsArticleHistory()
        {
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");

            short userId = short.Parse(_jwtTokenService.GetId(jwtTokenFromSession!));
            
            var newsArticleList = await _newsArticleService.GetNewsArticleAccordingToCreateById(userId);
            
            return View(newsArticleList);
        }
    }
}
