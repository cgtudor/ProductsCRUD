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
        public Task<ProductDomainModel> GetProductAsync(int ID);
        public int CreateProduct(ProductDomainModel productDomainModel);
        public void UpdateProduct(ProductDomainModel productDomainModel);
        public Task SaveChangesAsync();
    }
}
