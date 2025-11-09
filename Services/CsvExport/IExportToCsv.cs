using CSaver.DB.Models;

namespace CSaver.Services.CsvExport;

public interface IExportToCsv<T>
{
    Task<string> exportToFileAsync(string path, IEnumerable<T> objectList);
}