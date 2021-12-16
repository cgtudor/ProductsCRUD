using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.DTOs
{
    public class ProductAddStockDTO
    {
        public int ProductQuantityToAdd { get; set; }
    }
}
