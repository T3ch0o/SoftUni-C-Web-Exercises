namespace Chushka.Services.Interfaces
{
    using System.Collections.Generic;

    using Chushka.Models;
    using Chushka.Models.ViewModels;

    public interface IProductService
    {
        IEnumerable<Product> GetProducts();

        void AddProduct(CreateProductViewModel model);
    }
}
