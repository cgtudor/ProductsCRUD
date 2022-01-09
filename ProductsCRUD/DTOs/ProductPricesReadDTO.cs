using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.DTOs
{
    public class ProductPricesReadDTO
    {
        [Key]
        public int ProductPriceID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public DateTime PriceChangeDate { get; set; }
    }
}
