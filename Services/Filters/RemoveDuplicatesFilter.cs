using CSaver.DB.Models;
using CSaver.Services.CsvExport;

namespace CSaver.Services.Filters;

public class RemoveDuplicatesFilter: IFilter<Delivery>
{
    public List<Delivery> filter(List<Delivery> objectList)
    {
        objectList = objectList
            .GroupBy(d => new { d.tpep_pickup_datetime, d.tpep_dropoff_datetime, d.passenger_count })
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToList();
        
        return objectList;
    }
}