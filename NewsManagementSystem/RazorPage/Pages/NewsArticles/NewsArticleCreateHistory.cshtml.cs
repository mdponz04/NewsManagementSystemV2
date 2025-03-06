using BusinessLogic.Interfaces;
using Data.DTOs.NewsArticleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.NewsArticles
{
    public class NewsArticleCreateHistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IJwtTokenService _jwtTokenService;
        public NewsArticleCreateHistoryModel(INewsArticleService newsArticleService, IJwtTokenService jwtTokenService)
        {
            _newsArticleService = newsArticleService;
            _jwtTokenService = jwtTokenService;
        }

        public List<GetNewsArticleDTO> NewsArticles { get; set; }
        [Authorize(Roles ="1")]
        public async Task OnGetAsync(string? searchString)
        {
            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
            if(jwtTokenFromSession == null)
            {
                return;
            }
            short userId = short.Parse(_jwtTokenService.GetId(jwtTokenFromSession));
            NewsArticles = await _newsArticleService.GetNewsArticleAccordingToCreateById(userId);

            return;
        }
    }
}
