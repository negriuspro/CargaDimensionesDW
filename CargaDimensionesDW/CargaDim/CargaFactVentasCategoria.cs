using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaFactVentasCategoria
    {
        private readonly VentasDwContext _context;

        public CargaFactVentasCategoria(VentasDwContext context)
        {
            _context = context;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando fact_ventas_categoria...");

            var categoriaMap = await _context.DimCategorias
                .ToDictionaryAsync(c => c.Category!, c => c.id_categoria);

            var facts = await _context.FactVentas
                .Join(_context.DimProducts,
                    f => f.ProductID,
                    p => p.ProductID,
                    (f, p) => new { f, p.Category })
                .GroupBy(x => new { x.Category, x.f.id_fecha })
                .Select(g => new
                {
                    g.Key.Category,
                    g.Key.id_fecha,
                    TotalQuantity = g.Sum(x => x.f.Quantity),
                    TotalRevenue  = g.Sum(x => x.f.TotalPrice),
                    NumOrders     = g.Count()
                })
                .ToListAsync();

            var factVentasCategoria = facts.Select(g => new FactVentaCategoria
            {
                id_categoria  = categoriaMap[g.Category!],
                id_fecha      = g.id_fecha,
                TotalQuantity = g.TotalQuantity,
                TotalRevenue  = g.TotalRevenue,
                NumOrders     = g.NumOrders
            }).ToList();

            await _context.BulkInsertAsync(factVentasCategoria, new BulkConfig { PreserveInsertOrder = true });

            Console.WriteLine($"   {factVentasCategoria.Count} registros cargados en fact_ventas_categoria.");
        }
    }
}
