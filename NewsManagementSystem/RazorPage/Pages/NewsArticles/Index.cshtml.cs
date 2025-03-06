using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.NewsArticleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IJwtTokenService _jwtTokenService;
        public IndexModel(INewsArticleService newsArticleService, IJwtTokenService jwtTokenService)
        {
            _newsArticleService = newsArticleService;
            _jwtTokenService = jwtTokenService;
        }

        public List<GetNewsArticleDTO> NewsArticles { get; set; }
        [AllowAnonymous]
        public async Task OnGetAsync(string? searchString)
        {
            short? role;
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            if(jwtTokenFromSession == null)
            {
                role = null;
            }
            else
            {
                role = short.Parse(_jwtTokenService.GetRole(jwtTokenFromSession));
            }

            if (searchString != null)
            {
                NewsArticles = await _newsArticleService.GetNewsArticleBySearchString(searchString);
            }
            else
            {
                NewsArticles = await _newsArticleService.GetAllNewsArticle();
            }

            if (role == 2 || role == null)
            {
                NewsArticles = await _newsArticleService.GetActiveNewsArticleList(NewsArticles);
            }

            return;
        }

    }
}
