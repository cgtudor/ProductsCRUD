using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsCRUD.DomainModels
{
    public class ProductPricesDomainModel
    {
        [Key]
        public int ProductPriceID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public double ProductPrice{ get; set; }
        [Required]
        public DateTime PriceChangeDate { get; set; }
    }
}