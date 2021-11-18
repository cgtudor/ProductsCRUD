using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsCRUD.DomainModels
{
    public class ProductDomainModel
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public double ProductPrice{ get; set; }
        [Required]
        public String ProductName { get; set; }
        [Required]
        public String ProductDescription { get; set; }
    }
}