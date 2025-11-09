using CSaver.DB.Models;
using CSaver.Services.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CSaver.DB;
using Microsoft.EntityFrameworkCore;
using CSaver.Services.CsvImport;
using CSaver.Services.CsvExport;
using System.Threading.Tasks;

namespace CSaver;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var services = buildServices();

            var provider = services.BuildServiceProvider();

            if (args.Count() > 0)
            {
                string filePath = args.First();

                var app = provider.GetRequiredService<Application>();
                await app.Run(filePath: filePath);
            }
            else
            {
                throw new Exception("FilePath must not be empty: `dotnet run {filePath}`");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error!!");
            Console.WriteLine(ex.Message);
        }
    }


    private static ServiceCollection buildServices()
    {
        ServiceCollection services = new();

        services.AddTransient<ICSVImportService, CSVImportService>();
        services.AddTransient<IExportToCsv<Delivery>, ExportToCsv>();
        services.AddTransient<IFilter<Delivery>, RemoveDuplicatesFilter>();
        services.AddTransient<IFilter<Delivery>, RemoveDbDuplicatesFilter>();
        services.AddTransient<Application>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        var configurationString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configurationString));

        return services;
    }
}