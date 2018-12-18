namespace Eventures.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Eventures.Areas.Events.ViewModels;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services;
    using Eventures.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Shouldly;

    using Xunit;

    public class EventServiceTests
    {
        private readonly EventuresDbContext _db;

        private readonly IEventService _service;

        private readonly IServiceProvider _provider;

        public EventServiceTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EventuresDbContext>(options => options.UseInMemoryDatabase("EventuresTestDB"));

            _provider = services.BuildServiceProvider();

            _db = _provider.GetService<EventuresDbContext>();

            _service = new EventService(_provider.GetService<EventuresDbContext>(), null);
        }

        [Fact]
        public void All_WithNoExistingData_ShouldReturnEmptyCollection()
        {
            List<Event> result = _service.GetAllWithTicketPrice().ToList();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void All_WithExistingData_ShouldReturnAllOfExistingData()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EventuresDbContext>(options =>
                options.UseInMemoryDatabase("EventuresTestDB"));

            Event eventModel = new Event { Id = "Whatever", Name = "TestEvent", TotalTickets = 10 };
            _db.Events.Add(eventModel);
            _db.SaveChanges();

            List<Event> result = _service.GetAllWithTicketPrice().ToList();
            result.ShouldNotBeEmpty();

            Event entity = result[0];
            entity.ShouldBeSameAs(eventModel);
        }

        [Fact]
        public void GetOrderedEvents_WithNoExistingData_ShouldReturnEmptyCollection()
        {
            List<MyOrderedEventsViewModel> result = _service.GetOrderedEvents("User");
            result.ShouldBeEmpty();
        }

        [Fact]
        public void GetOrderedEvents_WithExistingData_ShouldReturnAllOrderedEvents()
        {
            Event eventModel = new Event { Id = "Whatever", Name = "TestEvent", TotalTickets = 10 };
            _db.Events.Add(eventModel);

            List<Order> orderModels = new List<Order>
            {
                new Order
                {
                    OrderOn = DateTime.Now,
                    TicketsCount = 5,
                    EventId = "Whatever",
                    CustomerId = "User"
                },
                new Order
                {
                    OrderOn = DateTime.Now,
                    TicketsCount = 5,
                    EventId = "Whatever",
                    CustomerId = "User"
                },
            };
            _db.Orders.AddRange(orderModels);
            _db.SaveChanges();

            List<MyOrderedEventsViewModel> result = _service.GetOrderedEvents("User");

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(2);
        }
    }
}