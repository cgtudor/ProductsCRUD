using System;
using ProductsCRUD.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Repositories.Interface
{
    public interface IProductsRepository
    {
        /// <summary>
        /// Get all the products from the database.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        public Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync();

        /// <summary>
        /// Get all the product price histories.
        /// </summary>
        /// <returns>A list of all the price change timestamps.</returns>
        public Task<IEnumerable<ProductPricesDomainModel>> GetAllProductsPricesAsync();

        /// <summary>
        /// Get the product with the ID specified.
        /// </summary>
        /// <param name="ID">ID of the product to be retrieved.</param>
        /// <returns>The product found or null if none was found.</returns>
        public Task<ProductDomainModel> GetProductAsync(int ID);

        /// <summary>
        /// Get a product's price history.
        /// </summary>
        /// <param name="ID">ID of the product to get the price history of.</param>
        /// <returns>A list of all price change timestamps for the requested product.</returns>
        public Task<IEnumerable<ProductPricesDomainModel>> GetProductPricesAsync(int ID);

        /// <summary>
        /// Creates a product in the database, as well as adding an entry to the price history table.
        /// </summary>
        /// <param name="productDomainModel">Domain model of product to be created.</param>
        /// <returns>ID of the newly created product.</returns>
        public int CreateProduct(ProductDomainModel productDomainModel);

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="productDomainModel">Domain model containing the new attributes.</param>
        public void UpdateProduct(ProductDomainModel productDomainModel);

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="ID">ID of the product to be deleted.</param>
        /// <returns>A task containing the deleted product's model or null if no product was found.</returns>
        public Task<ProductDomainModel> DeleteProductAsync(int ID);

        /// <summary>
        /// Add stock to a product in the database.
        /// </summary>
        /// <param name="ID">ID of the product to add stock to.</param>
        /// <param name="quantityToAdd">Amount of stock to add.</param>
        public void AddStock(int ID, int quantityToAdd);

        /// <summary>
        /// Commit changes to the database.
        /// </summary>
        /// <returns>Task result of the query.</returns>
        public Task SaveChangesAsync();
    }
}
