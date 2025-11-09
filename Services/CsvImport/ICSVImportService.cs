namespace CSaver.Services.CsvImport;

public interface ICSVImportService
{
    List<T> getEnteties<T>(string filePath, Dictionary<string, List<string>> mappings) where T : class, new();
}
