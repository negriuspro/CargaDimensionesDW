using CargaDimensionesDW.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.Data
{
    public class VentasDwContext : DbContext
    {
        public VentasDwContext(DbContextOptions<VentasDwContext> options) : base(options) { }

        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimProduct> DimProducts { get; set; }
        public DbSet<DimFecha> DimFecha { get; set; }
        public DbSet<FactVenta> FactVentas { get; set; }
    }
}
