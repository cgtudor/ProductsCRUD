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
            if (productDomainModel == null)
                throw new ArgumentNullException(nameof(productDomainModel), "The product model to be updated cannot be null");
            _context._products.Update(productDomainModel);
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

        public async void AddStock(int ID, int quantityToAdd)
        {
            var product = await _context._products
                        .FirstOrDefaultAsync(o => o.ProductID == ID);
            if (product == null)
                throw new ResourceNotFoundException();
            product.ProductQuantity += quantityToAdd;
        }
    }
}
