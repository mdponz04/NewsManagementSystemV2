using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Data.DTOs.SystemAccountDTOs;

namespace NewsManagementSystem.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly ILogger<AuthController> _logger; // Correctly injecting ILogger<T>

        // Constructor to inject the service and logger
        public AuthController(ISystemAccountService systemAccountService, ILogger<AuthController> logger)
        {
            _systemAccountService = systemAccountService;
            _logger = logger; // Set the logger
        }

        // Index Action (landing page for authentication)
        public IActionResult Index()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            ViewData["JwtToken"] = jwtToken; // Passing the JWT token to the view

            if (jwtToken == null)
            {
                // If no token is found, prompt for login or redirect
                _logger.LogInformation("No JWT Token found, redirecting to Login.");
                return RedirectToAction("Login");
            }

            // If token is present, proceed to the home page
            _logger.LogInformation("JWT Token found in session.");
            return View();
        }

        // GET: Display Login Form
        [HttpGet("login")]
        public IActionResult Login()
        {
            _logger.LogInformation("Navigating to Login page.");
            return View(); // Returns the login view
        }

        // POST: Handle Login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                // If model state is invalid, return to the login form with errors
                _logger.LogWarning("Login attempt with invalid model state.");
                return View(loginDTO);
            }

            try
            {
                // Check if credentials are valid and generate JWT
                var token = await _systemAccountService.Login(loginDTO);

                // Log to confirm if token is generated
                _logger.LogInformation("Generated JWT Token: {Token}", token);

                if (token != null)
                {
                    // Store JWT token in session
                    HttpContext.Session.SetString("jwt_token", token);

                    // Log session storage action
                    _logger.LogInformation("JWT Token stored in session");
                    var jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");
                    _logger.LogInformation("JWT Token from Session: {Token}", jwtTokenFromSession);

                    // Redirect to the Home page after successful login
                    _logger.LogInformation("Redirecting to Home page after successful login.");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Log if token is null
                    _logger.LogWarning("JWT Token generation failed for credentials: {AccountEmail}", loginDTO.AccountEmail);

                    ViewData["ErrorMessage"] = "Invalid credentials, please try again.";
                    return View(loginDTO); // Return to login form with error message
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, log it and return to the login form with error message
                _logger.LogError(ex, "Login error for credentials: {AccountEmail}", loginDTO.AccountEmail);
                ViewData["ErrorMessage"] = "Invalid credentials, please try again.";
                return View(loginDTO);
            }
        }

        // POST: Handle Logout
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove JWT token from the session on logout
            _logger.LogInformation("Logging out, removing JWT token from session.");
            HttpContext.Session.Remove("jwt_token");

            // Redirect to the login page after logout
            _logger.LogInformation("Redirecting to Login page after logout.");
            return RedirectToAction("Login", "Auth");
        }
    }
}
