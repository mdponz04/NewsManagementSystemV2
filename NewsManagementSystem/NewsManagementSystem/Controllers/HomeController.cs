using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "2")]
        public IActionResult SecurePage()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");

            // Decode the JWT token to extract the user's name
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(jwtTokenFromSession))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtTokenFromSession) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    // Assuming the user's name is stored in the "name" claim
                    userName = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                }
            }

            // Pass the user name to the view
            ViewData["UserName"] = userName;

            return View("SecurePage");
        }

    }
}
