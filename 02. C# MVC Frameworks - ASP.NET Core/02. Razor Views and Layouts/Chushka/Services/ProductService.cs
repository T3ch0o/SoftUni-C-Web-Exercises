using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chushka.Services
{
    using Chushka.Data;
    using Chushka.Models;
    using Chushka.Models.Enums;
    using Chushka.Models.ViewModels;
    using Chushka.Services.Interfaces;

    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext _db;

        public ProductService(ChushkaDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _db.Products;
        }

        public void AddProduct(CreateProductViewModel model)
        {
            if (Enum.TryParse(model.SelectedFoodType, out ProductType productType))
            {
                Product product = new Product
                {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Type = productType
                };

                _db.Products.Add(product);
                _db.SaveChanges();
            }
        }

        public Product GetProduct(int id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
