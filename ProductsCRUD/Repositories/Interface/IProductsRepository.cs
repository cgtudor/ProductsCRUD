using System;
using ProductsCRUD.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Repositories.Interface
{
    public interface IProductsRepository
    {
        public Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync();
        public Task<IEnumerable<ProductPricesDomainModel>> GetAllProductsPricesAsync();
        public Task<ProductDomainModel> GetProductAsync(int ID);
        public Task<IEnumerable<ProductPricesDomainModel>> GetProductPricesAsync(int ID);
        public int CreateProduct(ProductDomainModel productDomainModel);
        public void UpdateProduct(ProductDomainModel productDomainModel);
        public Task<ProductDomainModel> DeleteProductAsync(int ID);
        public void AddStock(int ID, int quantityToAdd);
        public Task SaveChangesAsync();
    }
}
