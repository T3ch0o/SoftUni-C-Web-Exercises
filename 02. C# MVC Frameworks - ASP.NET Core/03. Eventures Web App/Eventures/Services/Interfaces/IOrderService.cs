namespace Eventures.Services.Interfaces
{
    using System.Collections.Generic;

    using Eventures.Areas.Events.ViewModels;
    using Eventures.Models.ViewModels;

    public interface IOrderService
    {
        List<OrderViewModel> GetAllOrders();

        bool CreateOrder(OrderTicketViewModel model, string customerId);
    }
}
