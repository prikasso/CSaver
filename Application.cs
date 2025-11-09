using System.Text.Json;
using CSaver.DB.Models;
using CSaver.Services.CsvExport;
using CSaver.Services.CsvImport;
using CSaver.Services.Filters;
using System;
using System.IO;
using CSaver.DB;

namespace CSaver
{
    public class Application
    {
        private readonly ICSVImportService _csvImportService;
        private readonly IEnumerable<IFilter<Delivery>> _filters;
        private readonly IExportToCsv<Delivery> _exportToCsvService;
        private readonly ApplicationDbContext _context;

        public Application(ICSVImportService csvImportService, IEnumerable<IFilter<Delivery>> filters, IExportToCsv<Delivery> exportToCsvService, ApplicationDbContext context)
        {
            _csvImportService = csvImportService;
            _context = context;
            _filters = filters;
            _exportToCsvService = exportToCsvService;
        }

        public async Task Run(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new Exception("PathFile is a must!");

            Dictionary<string, List<string>> deliveryMappings = new()
            {
                { "tpep_pickup_datetime",  new() {"tpep_pickup_datetime"}  },
                { "tpep_dropoff_datetime", new() {"tpep_dropoff_datetime"} },
                { "passenger_count",       new() {"passenger_count"}       },
                { "trip_distance",         new() {"trip_distance"}         },
                { "store_and_fwd_flag",    new() {"store_and_fwd_flag"}    },
                { "PULocationID",          new() {"PULocationID"}          },
                { "DOLocationID",          new() {"DOLocationID"}          },
                { "fare_amount",           new() {"fare_amount"}           },
                { "tip_amount",            new() {"tip_amount"}            }
            };

            Console.WriteLine($"Importing all rows from csv file {filePath}");
            List<Delivery> deliveryList = _csvImportService.getEnteties<Delivery>(filePath, deliveryMappings);
            List<Delivery> deliveryListBeforeChanges = JsonSerializer.Deserialize<List<Delivery>>(JsonSerializer.Serialize<List<Delivery>>(deliveryList)) ?? new();

            foreach (var filter in _filters)
            {
                Console.WriteLine($"Running {filter.GetType().Name} filter");
                deliveryList = filter.filter(deliveryList);
            }

            string duplicatesFilePath = Path.Combine(Environment.CurrentDirectory, "duplicates.csv");

            if (deliveryList.Count > 0)
            {
                Console.WriteLine("Storing clear data to DB");
                await _context.AddRangeAsync(deliveryList);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Writing duplicates into {duplicatesFilePath}");
                await _exportToCsvService.exportToFileAsync(duplicatesFilePath, deliveryListBeforeChanges.Except(deliveryList));

                Console.WriteLine("Done!");
            }
            else
            {
                Console.WriteLine($"No unique rows was found");

                Console.WriteLine($"Writing duplicates into {duplicatesFilePath}");
                await _exportToCsvService.exportToFileAsync(duplicatesFilePath, deliveryListBeforeChanges);
            }
        }
    }
}