namespace Chushka.Services.Interfaces
{
    using System.Collections.Generic;

    using Chushka.Models;
    using Chushka.Models.ViewModels;

    public interface IProductService
    {
        IEnumerable<Product> GetProducts();

        void AddProduct(ProductViewModel model);

        void EditProduct(ProductViewModel model);

        void DeleteProduct(int id);

        Product GetProduct(int id);
    }
}
