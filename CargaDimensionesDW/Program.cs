using CargaDimensionesDW.CargaDim;
using CargaDimensionesDW.Data;
using CargaDimensionesDW.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
var csvPath = Environment.GetEnvironmentVariable("CSV_PATH")!;

var optionsBuilder = new DbContextOptionsBuilder<VentasDwContext>();
optionsBuilder.UseSqlServer(connectionString);

using var context = new VentasDwContext(optionsBuilder.Options);
var csvService = new CsvReaderService(csvPath);

Console.WriteLine("=== Inicio carga DW ventas_dw ===");
Console.WriteLine();

// 1. Limpiar tablas (orden inverso a las FK)
Console.WriteLine("Limpiando tablas...");
await context.FactVentasCategoria.ExecuteDeleteAsync();
await context.FactVentasCliente.ExecuteDeleteAsync();
await context.FactVentasProducto.ExecuteDeleteAsync();
await context.FactVentas.ExecuteDeleteAsync();
await context.DimCategorias.ExecuteDeleteAsync();
await context.DimFecha.ExecuteDeleteAsync();
await context.DimCustomers.ExecuteDeleteAsync();
await context.DimProducts.ExecuteDeleteAsync();
Console.WriteLine("  Tablas limpias.");
Console.WriteLine();

// 2. Cargar dimensiones
await new CargaCustomers(context, csvService).CargarAsync();
await new CargaProducts(context, csvService).CargarAsync();
await new CargaFecha(context, csvService).CargarAsync();
await new CargaCategoria(context).CargarAsync();
Console.WriteLine();

// 3. Cargar fact principal
await new CargaFactVentas(context, csvService).CargarAsync();
Console.WriteLine();

// 4. Cargar facts derivadas
await new CargaFactVentasProducto(context).CargarAsync();
await new CargaFactVentasCliente(context).CargarAsync();
await new CargaFactVentasCategoria(context).CargarAsync();

Console.WriteLine();
Console.WriteLine("=== Proceso completado exitosamente ===");
