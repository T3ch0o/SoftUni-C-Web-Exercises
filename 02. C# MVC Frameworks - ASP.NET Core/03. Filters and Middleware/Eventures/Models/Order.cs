namespace Eventures.Models
{
    using System;

    public class Order
    {
        public string Id { get; set; }

        public DateTime OrderOn { get; set; }

        public int TicketsCount { get; set; }

        public string EventId { get; set; }

        public string CustomerId { get; set; }
    }
}
