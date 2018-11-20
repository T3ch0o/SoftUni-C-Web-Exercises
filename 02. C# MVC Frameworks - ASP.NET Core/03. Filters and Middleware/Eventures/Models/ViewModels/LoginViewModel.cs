namespace Eventures.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
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

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
