namespace CakeWebApp.Models
{
    using System.Collections.Generic;

    public class Product : BaseModel<int>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<OrderProduct> Orders { get; set; } = new HashSet<OrderProduct>();
    }
}