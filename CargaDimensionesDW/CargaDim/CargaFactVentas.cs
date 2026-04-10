using CargaDimensionesDW.Data;
using CargaDimensionesDW.Models;
using CargaDimensionesDW.Services;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CargaDimensionesDW.CargaDim
{
    public class CargaFactVentas
    {
        private readonly VentasDwContext _context;
        private readonly CsvReaderService _csvService;

        public CargaFactVentas(VentasDwContext context, CsvReaderService csvService)
        {
            _context = context;
            _csvService = csvService;
        }

        public async Task CargarAsync()
        {
            Console.WriteLine("Cargando fact_ventas...");

            var orders = await _csvService.ReadOrdersAsync();
            var orderDetails = await _csvService.ReadOrderDetailsAsync();

            var fechaMap = await _context.DimFecha
                .ToDictionaryAsync(f => f.OrderDate.Date, f => f.id_fecha);

      
            var orderMap = orders.ToDictionary(o => o.OrderID);

            var facts = new List<FactVenta>();
            int omitidos = 0;

            foreach (var detail in orderDetails)
            {
                if (!orderMap.TryGetValue(detail.OrderID, out var order))
                {
                    Console.WriteLine($"  [!] Orden no encontrada: OrderID={detail.OrderID}");
                    omitidos++;
                    continue;
                }

                if (!fechaMap.TryGetValue(order.OrderDate.Date, out var idFecha))
                {
                    Console.WriteLine($"  [!] Fecha no encontrada: {order.OrderDate:yyyy-MM-dd}");
                    omitidos++;
                    continue;
                }

                facts.Add(new FactVenta
                {
                    ProductID = detail.ProductID,
                    CustomerID = order.CustomerID,
                    id_fecha = idFecha,
                    Quantity = detail.Quantity,
                    TotalPrice = detail.TotalPrice
                });
            }

            await _context.BulkInsertAsync(facts, new BulkConfig { PreserveInsertOrder = true });

            Console.WriteLine($"   {facts.Count} ventas cargadas. Omitidos: {omitidos}.");
        }
    }
}
