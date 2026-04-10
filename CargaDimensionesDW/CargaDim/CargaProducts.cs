using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using CargaDimensionesDW.Services;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaProducts
    {
        private readonly VentasDwContext _context;
        private readonly CsvReaderService _csvService;

        public CargaProducts(VentasDwContext context, CsvReaderService csvService)
        {
            _context = context;
            _csvService = csvService;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando productos...");

            var products = await _csvService.ReadProductsAsync();

            var dimProducts = products.Select(p => new DimProduct
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Category = p.Category,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();

            await _context.DimProducts.AddRangeAsync(dimProducts);
            await _context.SaveChangesAsync();

            Console.WriteLine($"   {dimProducts.Count} productos cargados.");
        }
    }
}
