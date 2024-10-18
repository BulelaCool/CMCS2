using Microsoft.AspNetCore.Mvc;
using CMCS2.Models;
using CMCS2.Data;
using System.Linq;

namespace CMCS2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ClaimsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View("~/Views/Home/SubmitClaim.cshtml"); // Path explicitly points to Home folder
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim)
        {
            // Calculate the TotalAmount before saving
            claim.TotalAmount = claim.HoursWorked * claim.HourlyRate;

            // Check if a supporting document was uploaded
            if (claim.SupportingDocuments == null || claim.SupportingDocuments.Length == 0)
            {
                ModelState.AddModelError("SupportingDocuments", "Please upload a supporting document.");
            }

            if (ModelState.IsValid)
            {
                _dbContext.Claims.Add(claim);
                _dbContext.SaveChanges();

                return RedirectToAction("ViewClaims", "Home"); // Redirect to Home controller's ViewClaims
            }

            // Log the error messages to understand the validation failure
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    // You can log to your logging framework, or simply to the console for debugging
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // Stay on SubmitClaim if there's an error
            return View("~/Views/Home/SubmitClaim.cshtml", claim);
        }

        [HttpGet]
        public IActionResult ViewClaims()
        {
            var claims = _dbContext.Claims.ToList();
            return View("~/Views/Home/ViewClaims.cshtml", claims); // Path explicitly points to Home folder
        }

        [HttpPost]
        public IActionResult UpdateClaimStatus(int id, string status)
        {
            // Find the claim by ID
            var claim = _dbContext.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                // Update the claim status
                claim.Status = status;
                _dbContext.SaveChanges();
            }

            // Redirect back to the ViewClaims page after updating
            return RedirectToAction("ViewClaims", "Home");
        }
    }
}
