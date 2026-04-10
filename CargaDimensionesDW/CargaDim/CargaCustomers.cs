using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using CargaDimensionesDW.Services;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaCustomers
    {
        private readonly VentasDwContext _context;
        private readonly CsvReaderService _csvService;

        public CargaCustomers(VentasDwContext context, CsvReaderService csvService)
        {
            _context = context;
            _csvService = csvService;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando clientes...");

            var customers = await _csvService.ReadCustomersAsync();

            var dimCustomers = customers.Select(c => new DimCustomer
            {
                CustomerID = c.CustomerID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                City = c.City,
                Country = c.Country
            }).ToList();

            await _context.DimCustomers.AddRangeAsync(dimCustomers);
            await _context.SaveChangesAsync();

            Console.WriteLine($"   {dimCustomers.Count} clientes cargados.");
        }
    }
}
