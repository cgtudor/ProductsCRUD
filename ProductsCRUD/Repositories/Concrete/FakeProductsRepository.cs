using ProductsCRUD.DomainModels;
using ProductsCRUD.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Repositories.Concrete
{
    public class FakeProductsRepository : IProductsRepository
    {
        public List<ProductDomainModel> _products = new List<ProductDomainModel>
        {
            new ProductDomainModel {ProductID = 0, ProductName = "TestProduct", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4},
            new ProductDomainModel {ProductID = 1, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45},
            new ProductDomainModel {ProductID = 2, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243},
            new ProductDomainModel {ProductID = 3, ProductName = "Nails", ProductDescription = "Pack of 50 nails.", ProductPrice = 4.5, ProductQuantity = 23},
            new ProductDomainModel {ProductID = 4, ProductName = "Candle", ProductDescription = "Aroma like you've never smelled before.", ProductPrice = 3.56, ProductQuantity = 43}
        };
        public int CreateProduct(ProductDomainModel productDomainModel)
        {
            int newOrderID = (_products.Count);
            productDomainModel.ProductID = newOrderID;
            _products.Add(productDomainModel);

            return newOrderID;
        }

        public Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<ProductDomainModel> GetProductAsync(int ID)
        {
            return Task.FromResult(_products.FirstOrDefault(o => o.ProductID == ID));
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public void UpdateProduct(ProductDomainModel productDomainModel)
        {
            var oldProductDomainModel = _products.FirstOrDefault(o => o.ProductID == productDomainModel.ProductID);
            _products.Remove(oldProductDomainModel);
            _products.Add(productDomainModel);
        }

        public Task<ProductDomainModel> DeleteProductAsync(int ID)
        {
            var productDomainModel = _products.FirstOrDefault(o => o.ProductID == ID);
            if (productDomainModel == null)
                return null;
            _products.Remove(productDomainModel);
            return Task.FromResult(productDomainModel);
        }
    }
}
