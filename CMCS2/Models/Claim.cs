using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace CMCS2.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public string? LecturerName { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        [BindNever]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        public string? SupportingDocuments { get; set; } // Path to the uploaded file
    }
}
