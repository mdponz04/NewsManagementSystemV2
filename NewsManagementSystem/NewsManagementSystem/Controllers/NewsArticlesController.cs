using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using BusinessLogic.Interfaces;
using Repositories.DTOs.NewsArticleDTOs;
﻿using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.NewsArticleDTOs;
using Repositories.PaggingItem;

namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly NewsManagementDbContext _context;
        
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;

        public NewsArticlesController(ITagService tagService, INewsArticleService newsArticleService, NewsManagementDbContext context)
        {
            _context = context;
            _newsArticleService = newsArticleService;
            _tagService = tagService;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {

            var newsArticles = await _newsArticleService.GetAllNewsArticle();
            return View(newsArticles);
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: NewsArticles/Create
        public async Task<IActionResult> Create()
        {
            // Populating ViewData for Category and Tags
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            var tags = await _tagService.GetAllTag();
            ViewData["Tags"] = tags;

            // Initialize an empty model to pass to the view
            var model = new PostNewsArticleDTO();
            return View(model);
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostNewsArticleDTO newsArticle)
        {
            if(!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _newsArticleService.GetCategoriesAsync(), "CategoryId", "CategoryDescription");
                ViewData["Tags"] = await _tagService.GetAllTag();

                return View(newsArticle);
            }
            //Selected tags from the form
            
            newsArticle.Tags = await _tagService.GetListTagByIdEntityType(newsArticle.SelectedTags);
            
            string id = await _newsArticleService.CreateNewsArticle(newsArticle);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            ViewData["CategoryId"] = new SelectList(await _newsArticleService.GetCategoriesAsync(), "CategoryId", "CategoryDescription");
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PutNewsArticleDTO newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }
            await _newsArticleService.UpdateNewsArticle(newsArticle);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: NewsArticles/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _newsArticleService.DeleteNewsArticle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
