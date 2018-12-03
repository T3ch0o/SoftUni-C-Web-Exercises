namespace Eventures.Services
{
    using System;

    using Eventures.Areas.Events.ViewModels;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services.Interfaces;

    public class OrderService : IOrderService
    {
        private readonly EventuresDbContext _db;

        public OrderService(EventuresDbContext db)
        {
            _db = db;
        }

        public void CreateOrder(OrderTicketViewModel model, string customerId)
        {
            Order order = new Order
            {
                OrderOn = DateTime.Now,
                    TicketsCount = model.Tickets,
                    EventId = model.EventId,
                    CustomerId = customerId
            };

            _db.Orders.Add(order);
            _db.SaveChanges();
        }
    }
}
