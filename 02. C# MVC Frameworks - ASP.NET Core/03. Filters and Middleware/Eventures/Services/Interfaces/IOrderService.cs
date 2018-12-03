namespace Eventures.Services.Interfaces
{
    using System.Collections.Generic;

    using Eventures.Areas.Events.ViewModels;
    using Eventures.Models.ViewModels;

    public interface IOrderService
    {
        List<OrderViewModel> GetAllOrder();

        void CreateOrder(OrderTicketViewModel model, string customerId);
    }
}
