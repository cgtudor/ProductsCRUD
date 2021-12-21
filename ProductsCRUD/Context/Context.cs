using Microsoft.EntityFrameworkCore;
using ProductsCRUD.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.Context
{
    public class Context : DbContext
    {
        virtual public DbSet<ProductDomainModel> _products { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public Context() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductDomainModel>()
                .HasData(
                new ProductDomainModel { ProductID = -1, ProductName = "Chocolate", ProductDescription = "Tasty and savory.", ProductPrice = 1.56, ProductQuantity = 4 },
                new ProductDomainModel { ProductID = -2, ProductName = "Heineken 0.5L", ProductDescription = "Blonde beer. Yummy.", ProductPrice = 2.56, ProductQuantity = 45 },
                new ProductDomainModel { ProductID = -3, ProductName = "Bread", ProductDescription = "Fresh.", ProductPrice = 0.56, ProductQuantity = 1243 },
                new ProductDomainModel { ProductID = -4, ProductName = "Nails", ProductDescription = "Pack of 50 nails.", ProductPrice = 4.5, ProductQuantity = 23 },
                new ProductDomainModel { ProductID = -5, ProductName = "Candle", ProductDescription = "Aroma like you've never smelled before.", ProductPrice = 3.56, ProductQuantity = 43 });
        }
    }
}
