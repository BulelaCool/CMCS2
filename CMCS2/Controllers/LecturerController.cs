using CMCS2.Data;
using CMCS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS2.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
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
                claim.DateSubmitted = DateTime.Now;
                claim.Status = "Pending";
                claim.TotalAmount = claim.HoursWorked * claim.HourlyRate;
                _context.Claims.Add(claim);
                _context.SaveChanges();

                return RedirectToAction("LecturerDashboard");
            }

            return View(claim);
        }
    }
}
