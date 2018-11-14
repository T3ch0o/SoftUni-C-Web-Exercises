namespace Chushka.Services
{
    public interface IOrderService
    {
        void CreateOrder(string userId, int productId);
    }
}
