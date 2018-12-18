namespace Eventures.Test.Controllers
{
    using System;
    using System.Collections.Generic;

    using Eventures.Controllers;
    using Eventures.Models.ViewModels;
    using Eventures.Services;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Shouldly;

    using Xunit;

    public class HomeControllerTests
    {
        private readonly IEventService _service;

        private readonly IServiceProvider _provider;

        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            ServiceCollection services = new ServiceCollection();
            Mock<OrderService> mockedOrderService = new Mock<OrderService>();
            List<OrderViewModel> orders = new List<OrderViewModel>
            {
                new OrderViewModel
                {
                    Customer = "User"
                }
            };

            mockedOrderService
                    .Setup(service => service.GetAllOrders())
                    .Returns(orders);

            services.AddScoped<HomeController>();
            services.AddScoped<IOrderService>(service => mockedOrderService.Object);

            _provider = services.BuildServiceProvider();

            _controller = _provider.GetService<HomeController>();
        }

        [Fact]
        public void Index_ShouldReturnViewResult()
        {
            IActionResult result = _controller.Index();
            result.ShouldBeOfType<ViewResult>();
        }
    }
}