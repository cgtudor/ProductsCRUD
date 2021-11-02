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
        public DbSet<ProductDomainModel> _products { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
