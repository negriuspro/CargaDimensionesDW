using CargaDimensionesDW.CargaDim;
using CargaDimensionesDW.Data;
using CargaDimensionesDW.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = config.GetConnectionString("VentasDW")!;
var csvPath = config["CsvPath"]!;

// Configurar DbContext
var optionsBuilder = new DbContextOptionsBuilder<VentasDwContext>();
optionsBuilder.UseSqlServer(connectionString);

using var context = new VentasDwContext(optionsBuilder.Options);
var csvService = new CsvReaderService(csvPath);

Console.WriteLine("=== Inicio carga DW ventas_dw ===");
Console.WriteLine();

// 1. Limpiar tablas 
Console.WriteLine("Limpiando tablas...");
await context.FactVentas.ExecuteDeleteAsync();
await context.DimFecha.ExecuteDeleteAsync();
await context.DimCustomers.ExecuteDeleteAsync();
await context.DimProducts.ExecuteDeleteAsync();
Console.WriteLine("  Tablas limpias.");
Console.WriteLine();

// 2. Cargar dimensiones
await new CargaCustomers(context, csvService).CargarAsync();
await new CargaProducts(context, csvService).CargarAsync();
await new CargaFecha(context, csvService).CargarAsync();
Console.WriteLine();

// 3. Cargar fact
await new CargaFactVentas(context, csvService).CargarAsync();

Console.WriteLine();
Console.WriteLine("=== Proceso completado exitosamente ===");
