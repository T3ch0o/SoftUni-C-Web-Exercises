namespace Eventures.Services.Interfaces
{
    using Eventures.Areas.Events.ViewModels;

    public interface IOrderService
    {
        void CreateOrder(OrderTicketViewModel model, string customerId);
    }
}
