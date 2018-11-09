namespace FDMC.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public string Nickname { get; set; }

        public override string UserName { get; set; }
    }
}
