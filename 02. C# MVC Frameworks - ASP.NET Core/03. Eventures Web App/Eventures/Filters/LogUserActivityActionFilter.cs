namespace Eventures.Filters
{
    using System;
    using System.Linq;

    using Eventures.Models;
    using Eventures.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class LogUserActivityActionFilter : ActionFilterAttribute
    {
        private readonly IEventService _eventService;

        private readonly ILogger<LogUserActivityActionFilter> _logger;

        public LogUserActivityActionFilter(IEventService eventService, ILogger<LogUserActivityActionFilter> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string username = context.HttpContext.User.Identity.Name;
            Event eventureEvent = _eventService.GetAllWithTicketPrice().FirstOrDefault();

            if (eventureEvent != null)
            {
                string logMessage = $"[{DateTime.Now}] User {username} viewed event {eventureEvent.Name} ( {eventureEvent.Start} / {eventureEvent.End})";

                _logger.LogInformation(logMessage);
            }

            base.OnActionExecuted(context);
        }
    }
}
