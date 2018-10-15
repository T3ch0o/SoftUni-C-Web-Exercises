﻿namespace CakesWebApp.Models
{
    public class User : BaseModel<int>
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order> Orders { get; set; }
    }
}