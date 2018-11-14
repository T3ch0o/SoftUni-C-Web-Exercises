namespace Chushka.Services
{
    using System;

    using Chushka.Data;
    using Chushka.Models;

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
    }
}
