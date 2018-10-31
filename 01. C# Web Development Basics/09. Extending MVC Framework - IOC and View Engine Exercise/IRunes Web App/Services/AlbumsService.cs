namespace IRunes.Services
{
    using IRunes.Services.Interfaces;

    internal class AlbumsService : IAlbumService
    {
        private const decimal Discount = 0.13M;

        public string GetPrice(decimal price)
        {
            price = price - (price * Discount);

            return price.ToString("F2");
        }
    }
}