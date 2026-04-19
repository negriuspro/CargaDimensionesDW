using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaCategoria
    {
        private readonly VentasDwContext _context;

        public CargaCategoria(VentasDwContext context)
        {
            _context = context;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando dim_categoria...");

            var categorias = await _context.DimProducts
                .Select(p => p.Category)
                .Distinct()
                .Select(c => new DimCategoria { Category = c })
                .ToListAsync();

            await _context.DimCategorias.AddRangeAsync(categorias);
            await _context.SaveChangesAsync();

            Console.WriteLine($"   {categorias.Count} categorias cargadas.");
        }
    }
}
