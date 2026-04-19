using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargaDimensionesDW.Models
{
    [Table("fact_ventas_cliente")]
    public class FactVentaCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int CustomerID { get; set; }
        public int id_fecha { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NumOrders { get; set; }
    }
}
