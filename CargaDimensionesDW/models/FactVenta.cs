using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargaDimensionesDW.Models
{
    [Table("fact_ventas")]
    public class FactVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_venta { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int id_fecha { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
