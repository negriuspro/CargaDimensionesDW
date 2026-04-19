using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaFactVentasCliente
    {
        private readonly VentasDwContext _context;

        public CargaFactVentasCliente(VentasDwContext context)
        {
            _context = context;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando fact_ventas_cliente...");

            var facts = await _context.FactVentas
                .GroupBy(f => new { f.CustomerID, f.id_fecha })
                .Select(g => new FactVentaCliente
                {
                    CustomerID = g.Key.CustomerID,
                    id_fecha = g.Key.id_fecha,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.TotalPrice),
                    NumOrders = g.Count()
                })
                .ToListAsync();

            await _context.BulkInsertAsync(facts, new BulkConfig { PreserveInsertOrder = true });

            Console.WriteLine($"   {facts.Count} registros cargados en fact_ventas_cliente.");
        }
    }
}
