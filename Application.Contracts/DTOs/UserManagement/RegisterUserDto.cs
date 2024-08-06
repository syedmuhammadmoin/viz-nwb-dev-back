using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RegisterUserDto
    {
        //[Required]
        //public int? EmployeeId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
        [Required]
        public IList<UserRolesDto> UserRoles { get; set; }
    }
    public class EditUserDto
    {
        public string UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        public IList<UserRolesDto> UserRoles { get; set; }
    }
    public class UserRolesDto
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
    public class ResetPasswordDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string AdminPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordDto
    {
        //public string loginUserId { get; set; }
        public string loginUserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }

    public class UserDto
    {
        public int? EmployeeId { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        public IList<UserRolesDto> UserRoles { get; set; }
    }
}