namespace Eventures.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    using Eventures.Areas.Event.ViewModels;
    using Eventures.Areas.Events.ViewModels;
    using Eventures.Models;

    public interface IEventService
    {
        IQueryable<Event> All();

        void Create(EventViewModel model);

        List<MyOrderedEventsViewModel> GetOrderedEvents(string userId);
    }
}
