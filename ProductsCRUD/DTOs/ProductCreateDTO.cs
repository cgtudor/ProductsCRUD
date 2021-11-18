using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsCRUD.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public String ProductName { get; set; }
        [Required]
        public String ProductDescription { get; set; }
    }
}
