
using CSaver.DB;
using CSaver.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace CSaver.Services.Filters;


public class RemoveDbDuplicatesFilter : IFilter<Delivery>
{
    private readonly ApplicationDbContext _context;

    public RemoveDbDuplicatesFilter(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<Delivery> filter(List<Delivery> objectList)
    {
        objectList = objectList
            .Where(
                p => _context.Deliveries
                        .Where(d => d.tpep_pickup_datetime == p.tpep_pickup_datetime && d.tpep_dropoff_datetime == p.tpep_dropoff_datetime && d.passenger_count == p.passenger_count)
                        .Count() == 0
            )
            .ToList();
        
        return objectList;
    }
}