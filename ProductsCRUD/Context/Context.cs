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
        virtual public DbSet<ProductPricesDomainModel> _productPrices { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public Context() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductDomainModel>()
                .HasData(
                new ProductDomainModel { ProductID = 1, ProductName = "Rippled Screen Protector", ProductDescription = "For his or her sensory pleasure. Fits few known smartphones.", ProductPrice = 8.29, ProductQuantity = 4 },
                new ProductDomainModel { ProductID = 2, ProductName = "Wrap it and Hope Cover", ProductDescription = "Poor quality fake faux leather cover, loose enough to fit any mobile device.", ProductPrice = 5.78, ProductQuantity = 45 },
                new ProductDomainModel { ProductID = 3, ProductName = "Chocolate Cover", ProductDescription = "Purchase your favourite chocolate and use the provided heating element t melt it into the perfect cover for your phone.", ProductPrice = 11.82, ProductQuantity = 1243 },
                new ProductDomainModel { ProductID = 4, ProductName = "Water Bath Case", ProductDescription = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", ProductPrice = 16.83, ProductQuantity = 23 },
                new ProductDomainModel { ProductID = 5, ProductName = "Smartphone Car Holder", ProductDescription = "Keep your smartphone handsfree with this large assembly that attaches to your rear window wiper.", ProductPrice = 97.02, ProductQuantity = 43 });

            builder.Entity<ProductPricesDomainModel>()
                .HasData(
                new ProductPricesDomainModel { ProductPriceID = 1, ProductID = 6, ProductPrice = 10, PriceChangeDate = new DateTime(2020, 1, 1) },
                new ProductPricesDomainModel { ProductPriceID = 2, ProductID = 7, ProductPrice = 8.29, PriceChangeDate = new DateTime(2020, 1, 26) },
                new ProductPricesDomainModel { ProductPriceID = 3, ProductID = 8, ProductPrice = 11.82, PriceChangeDate = new DateTime(2020, 1, 1) },
                new ProductPricesDomainModel { ProductPriceID = 4, ProductID = 9, ProductPrice = 10, PriceChangeDate = new DateTime(2020, 1, 1) },
                new ProductPricesDomainModel { ProductPriceID = 5, ProductID = 10, ProductPrice = 5.78, PriceChangeDate = new DateTime(2020, 1, 21) });
        }
    }
}
