namespace Eventures.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
