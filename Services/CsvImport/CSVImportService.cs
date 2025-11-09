using System.Globalization;

namespace CSaver.Services.CsvImport;

public class CSVImportService : ICSVImportService
{
    public List<T> getEnteties<T>(string filePath, Dictionary<string, List<string>> mappings)
        where T : class, new()
    {
        var list = new List<T>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        using var reader = new StreamReader(filePath);
        string? headerLine = reader.ReadLine();
        if (headerLine == null)
            return list;

        var headers = headerLine.Split(',')
                                .Select(h => h.Trim().ToLowerInvariant())
                                .ToArray();

        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                .Where(p => p.CanWrite)
                                .ToDictionary(p => p.Name.ToLowerInvariant(), p => p);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            T instance = new T();
            var values = line.Split(',');

            for (int i = 0; i < headers.Length && i < values.Length; i++)
            {
                string header = headers[i];

                if (mappings != null && mappings.TryGetValue(headers[i], out var mappedProps))
                {
                    header = mappedProps.First();
                }

                if (properties.TryGetValue(header.ToLowerInvariant(), out var prop))
                {
                    try
                    {
                        object? convertedValue = Convert.ChangeType(
                            values[i],
                            Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType,
                            CultureInfo.InvariantCulture
                        );

                        prop.SetValue(instance, convertedValue);
                    }
                    catch { }
                }
            }

            list.Add(instance);
        }

        return list;
    }

}