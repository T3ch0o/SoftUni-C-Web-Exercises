using Microsoft.AspNetCore.Mvc;

namespace Eventures.Controllers
{
    using System.Collections.Generic;

    using Eventures.Models.ViewModels;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult All()
        {
            List<OrderViewModel> orders = _orderService.GetAllOrder();

            return View(orders);
        }
    }
}