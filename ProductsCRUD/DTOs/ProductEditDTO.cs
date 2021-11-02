using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.DTOs
{
    public class ProductEditDTO
    {
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public String ProductName { get; set; }
        public String ProductDescription { get; set; }
    }
}
