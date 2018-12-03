namespace Eventures.Areas.Events.ViewModels
{
    using System;

    public class MyOrderedEventsViewModel
    {
        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TicketsCount { get; set; }
    }
}
