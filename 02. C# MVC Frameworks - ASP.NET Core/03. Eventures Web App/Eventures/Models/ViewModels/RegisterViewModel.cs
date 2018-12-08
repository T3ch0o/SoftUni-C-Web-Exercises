namespace Eventures.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [MinLength(3, ErrorMessage = "Username is too short.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(3, ErrorMessage = "Password is too short.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Universal Citizen Number")]
        [MinLength(10)]
        [MaxLength(10)]
        public string UniversalCitizenNumber { get; set; }
    }
}
