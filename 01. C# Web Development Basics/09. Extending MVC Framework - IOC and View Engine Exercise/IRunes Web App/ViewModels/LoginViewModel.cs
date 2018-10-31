namespace IRunes.ViewModels
{
    using System.Collections.Generic;

    internal class LoginViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}