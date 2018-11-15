namespace Chushka.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chushka.Data;
    using Chushka.Models;

    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly ChushkaDbContext _db;

        public OrderService(ChushkaDbContext db)
        {
            _db = db;
        }

        public void CreateOrder(string userId, int productId)
        {
            Order order = new Order
            {
                ProductId = productId,
                ClientId = userId,
                OrderOn = DateTime.Now
            };

            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public List<Order> GetOrders()
        {
            return _db.Orders
                      .Include(order => order.Product)
                      .Include(order => order.Client)
                      .ToList();
        }
    }
}
