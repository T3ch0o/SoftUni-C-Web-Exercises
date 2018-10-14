namespace CakeWebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Order : BaseModel<int>
    {
        public int UserId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; } = new HashSet<OrderProduct>();
    }
}