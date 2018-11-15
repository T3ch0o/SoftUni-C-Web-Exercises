namespace Chushka.Services
{
    using System.Collections.Generic;

    using Chushka.Models;

    public interface IOrderService
    {
        void CreateOrder(string userId, int productId);

        List<Order> GetOrders();
    }
}
