namespace Eventures.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Eventures.Areas.Events.ViewModels;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Models.ViewModels;
    using Eventures.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly EventuresDbContext _db;

        public OrderService(EventuresDbContext db)
        {
            _db = db;
        }

        public List<OrderViewModel> GetAllOrders()
        {
            List<OrderViewModel> orders = new List<OrderViewModel>();

            foreach (Order order in _db.Orders.Include(order => order.Customer).Include(order => order.Event))
            {
                orders.Add(new OrderViewModel
                {
                     EventName = order.Event.Name,
                     Customer = order.Customer.UserName,
                     OrderOn = order.OrderOn
                });
            }

            return orders;
        }

        public bool CreateOrder(OrderTicketViewModel model, string customerId)
        {
            Event @event = _db.Events.FirstOrDefault(e => e.Id == model.EventId);

            if (@event != null && @event.TotalTickets >= model.Tickets)
            {
                Order order = new Order
                {
                    OrderOn = DateTime.Now,
                    TicketsCount = model.Tickets,
                    EventId = model.EventId,
                    CustomerId = customerId
                };

                @event.TotalTickets -= model.Tickets;

                _db.Orders.Add(order);
                _db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
