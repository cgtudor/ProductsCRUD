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
        public List<ProductPricesDomainModel> _productPrices = new List<ProductPricesDomainModel>
        {
            new ProductPricesDomainModel {ProductPriceID = 0, ProductID = 0, ProductPrice = 10, PriceChangeDate = new DateTime(2020, 1, 1)},
            new ProductPricesDomainModel {ProductPriceID = 1, ProductID = 0, ProductPrice = 1.56, PriceChangeDate = new DateTime(2020, 1, 26)},
            new ProductPricesDomainModel {ProductPriceID = 2, ProductID = 1, ProductPrice = 2.56, PriceChangeDate = new DateTime(2020, 1, 1)},
            new ProductPricesDomainModel {ProductPriceID = 3, ProductID = 2, ProductPrice = 10, PriceChangeDate = new DateTime(2020, 1, 1)},
            new ProductPricesDomainModel {ProductPriceID = 4, ProductID = 2, ProductPrice = 0.56, PriceChangeDate = new DateTime(2020, 1, 21)}
        };

        /// <summary>
        /// Creates a product in the fake database, as well as adding an entry to the price history table.
        /// </summary>
        /// <param name="productDomainModel">Domain model of product to be created.</param>
        /// <returns>ID of the newly created product.</returns>
        public int CreateProduct(ProductDomainModel productDomainModel)
        {
            int newOrderID = (_products.Count);
            productDomainModel.ProductID = newOrderID;
            _products.Add(productDomainModel);
            
            // Record the initial price of the product in the price history table.
            _productPrices.Add(new ProductPricesDomainModel
            {
                ProductPriceID = _productPrices.Count,
                ProductID = productDomainModel.ProductID,
                ProductPrice = productDomainModel.ProductPrice,
                PriceChangeDate = DateTime.Now
            });

            return newOrderID;
        }

        /// <summary>
        /// Get all the products in the fake database.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        public Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        /// <summary>
        /// Get the product with the ID specified.
        /// </summary>
        /// <param name="ID">ID of the product to be retrieved.</param>
        /// <returns>The product found.</returns>
        public Task<ProductDomainModel> GetProductAsync(int ID)
        {
            return Task.FromResult(_products.FirstOrDefault(o => o.ProductID == ID));
        }

        /// <summary>
        /// Commit changes to the database.
        /// </summary>
        /// <returns>Completed task result.</returns>
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Update a product in the fake database.
        /// </summary>
        /// <param name="productDomainModel">Domain model containing the new attributes.</param>
        public void UpdateProduct(ProductDomainModel productDomainModel)
        {
            var oldProductDomainModel = _products.FirstOrDefault(o => o.ProductID == productDomainModel.ProductID);
            _products.Remove(oldProductDomainModel);
            _products.Add(productDomainModel);
            _productPrices.Add(new ProductPricesDomainModel { ProductPriceID = _productPrices.Count, ProductID = oldProductDomainModel.ProductID,
                ProductPrice = productDomainModel.ProductPrice, PriceChangeDate = DateTime.Now });
        }

        /// <summary>
        /// Delete a product from the fake database.
        /// </summary>
        /// <param name="ID">ID of the product to be deleted.</param>
        /// <returns>A task containing the deleted product's model.</returns>
        public Task<ProductDomainModel> DeleteProductAsync(int ID)
        {
            var productDomainModel = _products.FirstOrDefault(o => o.ProductID == ID);
            if (productDomainModel == null)
                return null;
            _products.Remove(productDomainModel);
            return Task.FromResult(productDomainModel);
        }

        /// <summary>
        /// Add stock to a product in the fake database.
        /// </summary>
        /// <param name="ID">ID of the product to add stock to.</param>
        /// <param name="quantityToAdd">Amount of stock to add.</param>
        public void AddStock(int ID, int quantityToAdd)
        {
            var productModel = _products.FirstOrDefault(o => o.ProductID == ID);
            productModel.ProductQuantity += quantityToAdd;
        }

        /// <summary>
        /// Get all the product price histories.
        /// </summary>
        /// <returns>A list of all the price change timestamps.</returns>
        public Task<IEnumerable<ProductPricesDomainModel>> GetAllProductsPricesAsync()
        {
            return Task.FromResult(_productPrices.AsEnumerable());
        }

        /// <summary>
        /// Get a product's price history.
        /// </summary>
        /// <param name="ID">ID of the product to get the price history of.</param>
        /// <returns>A list of all price change timestamps for the requested product.</returns>
        public Task<IEnumerable<ProductPricesDomainModel>> GetProductPricesAsync(int ID)
        {
            return Task.FromResult(_productPrices.Where(o => o.ProductID == ID).AsEnumerable());
        }
    }
}
