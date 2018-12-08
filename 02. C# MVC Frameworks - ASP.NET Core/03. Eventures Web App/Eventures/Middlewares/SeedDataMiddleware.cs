namespace Eventures.Middlewares
{
    using System;
    using System.Threading.Tasks;

    using Eventures.Data;
    using Eventures.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Internal;

    public class SeedDataMiddleware
    {
        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
                                      EventuresDbContext db,
                                      RoleManager<IdentityRole> roleManager)
        {
            if (!db.Events.Any())
            {
                SeedEvents(db);
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            await _next(context);
        }

        private static void SeedEvents(EventuresDbContext db)
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
