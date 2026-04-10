using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargaDimensionesDW.Models
{
    [Table("dim_products")]
    public class DimProduct
    {
        [Key]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
