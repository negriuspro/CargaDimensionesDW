using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using CargaDimensionesDW.Services;
using System.Globalization;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaFecha
    {
        private readonly VentasDwContext _context;
        private readonly CsvReaderService _csvService;

        public CargaFecha(VentasDwContext context, CsvReaderService csvService)
        {
            _context = context;
            _csvService = csvService;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando fechas...");

            var orders = await _csvService.ReadOrdersAsync();

            var fechas = orders
                .Select(o => o.OrderDate.Date)
                .Distinct()
                .Select(date => new DimFecha
                {
                    OrderDate = date,
                    Day = date.Day,
                    Month = date.Month,
                    MonthName = date.ToString("MMMM", new CultureInfo("en-US")),
                    Quarter = (date.Month - 1) / 3 + 1,
                    Year = date.Year
                }).ToList();

            await _context.DimFecha.AddRangeAsync(fechas);
            await _context.SaveChangesAsync();

            Console.WriteLine($"   {fechas.Count} fechas cargadas.");
        }
    }
}
