using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs
{
    public class CreateApplicantRelativeDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        [Required]
        public Relationship? Relationship { get; set; }
        [Required]
        [MaxLength(20)]
        public string CNIC { get; set; }
        [Required]
        [MaxLength(15)]
        public string ContactNo { get; set; }
        [Required]
        [MaxLength(200)]
        public string Occupation { get; set; }
    }
}
