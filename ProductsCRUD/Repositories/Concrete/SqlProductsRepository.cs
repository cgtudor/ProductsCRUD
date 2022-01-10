using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Controllers;
using ProductsCRUD.DomainModels;
using ProductsCRUD.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Repositories.Concrete
{
    public class SqlProductsRepository : IProductsRepository
    {
        private readonly Context.Context _context;

        public SqlProductsRepository(Context.Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a product in the database, as well as adding an entry to the price history table.
        /// </summary>
        /// <param name="productDomainModel">Domain model of product to be created.</param>
        /// <returns>ID of the newly created product.</returns>
        public int CreateProduct(ProductDomainModel productDomainModel)
        {
            var newProduct = _context._products.Add(productDomainModel).Entity;

            // Record the initial price of the product in the price history table.
            _context._productPrices.Add(new ProductPricesDomainModel
            {
                ProductID = newProduct.ProductID,
                ProductPrice = newProduct.ProductPrice,
                PriceChangeDate = DateTime.Now
            });
            return newProduct.ProductID;
        }

        /// <summary>
        /// Get all the products from the database.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        public async Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync()
        {
            return await _context._products.ToListAsync();
        }

        /// <summary>
        /// Get the product with the ID specified.
        /// </summary>
        /// <param name="ID">ID of the product to be retrieved.</param>
        /// <returns>The product found or null if none was found.</returns>
        public async Task<ProductDomainModel> GetProductAsync(int ID)
        {
            return await _context._products.FirstOrDefaultAsync(o => o.ProductID == ID);
        }

        /// <summary>
        /// Commit changes to the database.
        /// </summary>
        /// <returns>Task result of the query.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="productDomainModel">Domain model containing the new attributes.</param>
        public void UpdateProduct(ProductDomainModel productDomainModel)
        {
            if (productDomainModel == null)
                throw new ArgumentNullException(nameof(productDomainModel), "The product model to be updated cannot be null");
            _context._products.Update(productDomainModel);
            _context._productPrices.Add(new ProductPricesDomainModel
            {
                ProductID = productDomainModel.ProductID,
                ProductPrice = productDomainModel.ProductPrice,
                PriceChangeDate = DateTime.Now
            });
        }


        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="ID">ID of the product to be deleted.</param>
        /// <returns>A task containing the deleted product's model or null if no product was found.</returns>
        public async Task<ProductDomainModel> DeleteProductAsync(int ID)
        {
            var productDomainModel = await _context._products
                .FirstOrDefaultAsync(o => o.ProductID == ID);
            if (productDomainModel != null)
            {
                _context._products.Remove(productDomainModel);
                return productDomainModel;
            }
            
            return null;
        }

        /// <summary>
        /// Add stock to a product in the database.
        /// </summary>
        /// <param name="ID">ID of the product to add stock to.</param>
        /// <param name="quantityToAdd">Amount of stock to add.</param>
        public async void AddStock(int ID, int quantityToAdd)
        {
            var product = await _context._products
                        .FirstOrDefaultAsync(o => o.ProductID == ID);
            if (product == null)
                throw new ResourceNotFoundException();
            product.ProductQuantity += quantityToAdd;
        }

        /// <summary>
        /// Get all the product price histories.
        /// </summary>
        /// <returns>A list of all the price change timestamps.</returns>
        public async Task<IEnumerable<ProductPricesDomainModel>> GetAllProductsPricesAsync()
        {
            return await _context._productPrices.ToListAsync();
        }

        /// <summary>
        /// Get a product's price history.
        /// </summary>
        /// <param name="ID">ID of the product to get the price history of.</param>
        /// <returns>A list of all price change timestamps for the requested product.</returns>
        public async Task<IEnumerable<ProductPricesDomainModel>> GetProductPricesAsync(int ID)
        {
            return await _context._productPrices.Where(p => p.ProductID == ID).ToListAsync();
        }
    }
}
