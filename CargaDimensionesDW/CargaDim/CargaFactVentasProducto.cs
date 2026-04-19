using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaFactVentasProducto
    {
        private readonly VentasDwContext _context;

        public CargaFactVentasProducto(VentasDwContext context)
        {
            _context = context;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando fact_ventas_producto...");

            var facts = await _context.FactVentas
                .GroupBy(f => new { f.ProductID, f.id_fecha })
                .Select(g => new FactVentaProducto
                {
                    ProductID = g.Key.ProductID,
                    id_fecha = g.Key.id_fecha,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.TotalPrice),
                    NumOrders = g.Count()
                })
                .ToListAsync();

            await _context.BulkInsertAsync(facts, new BulkConfig { PreserveInsertOrder = true });

            Console.WriteLine($"   {facts.Count} registros cargados en fact_ventas_producto.");
        }
    }
}
