namespace Chushka.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public string ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public ApplicationUser Client { get; set; }

        public DateTime OrderOn { get; set; }
    }
}
