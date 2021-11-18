using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.DTOs
{
    public class ProductReadDTO
    {
        [Key]
        public int ProductID { get; set; }
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
