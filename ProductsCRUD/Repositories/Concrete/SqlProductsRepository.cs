using Microsoft.EntityFrameworkCore;
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

        public int CreateProduct(ProductDomainModel productDomainModel)
        {
            return _context._products.Add(productDomainModel).Entity.ProductID;
        }

        public async Task<IEnumerable<ProductDomainModel>> GetAllProductsAsync()
        {
            return await _context._products.ToListAsync();
        }

        public async Task<ProductDomainModel> GetProductAsync(int ID)
        {
            return await _context._products.FirstOrDefaultAsync(o => o.ProductID == ID);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateProduct(ProductDomainModel productDomainModel)
        {
            
        }

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

        public void AddStock(int ID, int quantityToAdd)
        {
            var product = _context._products
                        .FirstOrDefault(o => o.ProductID == ID);
            product.ProductQuantity += quantityToAdd;
        }
    }
}
