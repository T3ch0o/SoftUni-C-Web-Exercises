namespace Eventures.Middlewares
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Eventures.Data;
    using Eventures.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public class SeedDataMiddleware
    {
        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, EventuresDbContext db)
        {
            if (!db.Events.Any())
            {
                SeedEvents(db);
            }

            await _next(context);
        }

        private void SeedEvents(EventuresDbContext db)
        {
            for (int i = 1; i <= 10; i++)
            {
                Event sampleEvent = new Event
                {
                    Name = $"Sample event number {i}",
                    Place = $"Sample place address Maior N{i}",
                    Start = DateTime.Now.Add(TimeSpan.FromDays(i)),
                    End = DateTime.Now.Add(TimeSpan.FromDays(i * 2)),
                    TicketPrice = 10 + i,
                    TotalTickets = i * 10
                };

                db.Events.Add(sampleEvent);
            }

            db.SaveChanges();
        }
    }
}
