using CargaDimensionesDW.Models.Csv;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CargaDimensionesDW.Services
{
    public class CsvReaderService
    {
        private readonly string _csvPath;

        public CsvReaderService(string csvPath)
        {
            _csvPath = csvPath;
        }

        public Task<List<CustomerCsv>> ReadCustomersAsync()
            => ReadFileAsync<CustomerCsv>("customers.csv");

        public Task<List<ProductCsv>> ReadProductsAsync()
            => ReadFileAsync<ProductCsv>("products.csv");

        public Task<List<OrderCsv>> ReadOrdersAsync()
            => ReadFileAsync<OrderCsv>("orders.csv");

        public Task<List<OrderDetailCsv>> ReadOrderDetailsAsync()
            => ReadFileAsync<OrderDetailCsv>("order_details.csv");

        private Task<List<T>> ReadFileAsync<T>(string fileName)
        {
            var filePath = Path.Combine(_csvPath, fileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<T>().ToList();
            return Task.FromResult(records);
        }
    }
}
