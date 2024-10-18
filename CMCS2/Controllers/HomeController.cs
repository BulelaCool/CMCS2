using CMCS2.Data;
using CMCS2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Contract_Monthly_Claim_System__CMCS_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim)
        {
            if (ModelState.IsValid)
            {
                // Add the claim to the database
                _context.Claims.Add(claim);
                _context.SaveChanges();

                // Redirect to the ViewClaims action to show the submitted claims
                return RedirectToAction("ViewClaims");
            }

            // If the model state is invalid, return the same view with error messages
            return View(claim);
        }

        [HttpGet]
        public IActionResult ViewClaims()
        {
            // Retrieve all claims from the database
            var claims = _context.Claims.ToList();
            return View("ViewClaims", claims); // This ensures it loads ViewClaims.cshtml from the Home folder
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
