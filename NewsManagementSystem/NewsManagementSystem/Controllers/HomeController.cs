using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public HomeController(ILogger<HomeController> logger, IJwtTokenService jwtTokenService)
        {
            _logger = logger;
            _jwtTokenService = jwtTokenService;
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

            // Retrieve claims using the JWT token service
            string userName = _jwtTokenService.GetName(jwtTokenFromSession!);
            string userEmail = _jwtTokenService.GetEmail(jwtTokenFromSession!);
            string userRole = _jwtTokenService.GetRole(jwtTokenFromSession!);
            string userId = _jwtTokenService.GetId(jwtTokenFromSession!);

            // Pass the user data to the view
            ViewData["UserName"] = userName;
            ViewData["UserEmail"] = userEmail;
            ViewData["UserRole"] = userRole;
            ViewData["UserId"] = userId;

            return View("SecurePage");
        }

    }
}
