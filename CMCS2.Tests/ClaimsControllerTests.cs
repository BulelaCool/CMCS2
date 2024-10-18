using CMCS2.Controllers;
using CMCS2.Data;
using CMCS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CMCS2.Tests
{
    public class ClaimsControllerTests
    {
        private readonly ClaimsController _controller;
        private readonly ApplicationDbContext _context;

        public ClaimsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ClaimsController(_context);
        }

        [Fact]
        public void SubmitClaim_ValidClaim_RedirectsToViewClaims()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 15.00m,
                SupportingDocuments = "document_path"
            };

            // Act
            var result = _controller.SubmitClaim(claim) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ViewClaims", result.ActionName);
        }

        [Fact]
        public void SubmitClaim_InvalidClaim_ReturnsViewWithErrors()
        {
            // Arrange
            var claim = new Claim
            {
                HoursWorked = 10,
                HourlyRate = 15.00m // Missing LecturerName
            };
            _controller.ModelState.AddModelError("LecturerName", "Required");

            // Act
            var result = _controller.SubmitClaim(claim) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void SubmitClaim_NoSupportingDocument_ReturnsError()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 15.00m,
                SupportingDocuments = null // No document uploaded
            };
            _controller.ModelState.AddModelError("SupportingDocuments", "Please upload a supporting document.");

            // Act
            var result = _controller.SubmitClaim(claim) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid);
            Assert.Contains(result.ViewData.ModelState, x => x.Key == "SupportingDocuments");
        }

        [Fact]
        public void ViewClaims_ReturnsClaimsView()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 15.00m
            };
            _context.Claims.Add(claim);
            _context.SaveChanges();

            // Act
            var result = _controller.ViewClaims() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Claim>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void UpdateClaimStatus_ValidId_UpdatesStatus()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 15.00m
            };
            _context.Claims.Add(claim);
            _context.SaveChanges();

            // Act
            var result = _controller.UpdateClaimStatus(claim.Id, "Approved");

            // Assert
            var updatedClaim = _context.Claims.First(c => c.Id == claim.Id);
            Assert.Equal("Approved", updatedClaim.Status);
        }

        [Fact]
        public void UpdateClaimStatus_InvalidId_DoesNotChangeStatus()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 15.00m
            };
            _context.Claims.Add(claim);
            _context.SaveChanges();

            // Act
            _controller.UpdateClaimStatus(claim.Id + 1, "Approved"); // Invalid ID

            // Assert
            var unchangedClaim = _context.Claims.First(c => c.Id == claim.Id);
            Assert.Equal("Pending", unchangedClaim.Status);
        }

    }
}
