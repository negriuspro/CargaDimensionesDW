using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargaDimensionesDW.Models
{
    [Table("dim_fecha")]
    public class DimFecha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_fecha { get; set; }
        public DateTime OrderDate { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string? MonthName { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
    }
}
