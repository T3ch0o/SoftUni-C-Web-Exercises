namespace Chushka.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }

        public DateTime OrderOn { get; set; }
    }
}
