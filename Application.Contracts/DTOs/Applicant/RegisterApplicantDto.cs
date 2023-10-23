using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class RegisterApplicantDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string FatherName { get; set; }
        [Required]
        [MaxLength(20)]
        public string CNIC { get; set; }
        [Required]
        [MaxLength(15)]
        public string ContactNo { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
