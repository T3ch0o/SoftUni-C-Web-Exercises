namespace Chushka.Controllers
{
    using System.Collections.Generic;

    using Chushka.Models;
    using Chushka.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly IOrderService _orderServices;

        public OrdersController(IOrderService orderServices)
        {
            _orderServices = orderServices;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            List<Order> orders = _orderServices.GetOrders();

            return View(orders);
        }
    }
}