namespace Eventures.Services.Interfaces
{
    using System.Collections.Generic;
    using Eventures.Models;

    public interface IEventService
    {
        IEnumerable<Event> All();
    }
}
