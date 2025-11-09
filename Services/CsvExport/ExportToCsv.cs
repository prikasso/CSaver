using System.Text;
using CSaver.DB.Models;

namespace CSaver.Services.CsvExport;


public class ExportToCsv : IExportToCsv<Delivery>
{
    public async Task<string> exportToFileAsync(string path, IEnumerable<Delivery> objectList)
    {
        // not efficient but fast way =(
        // can be a problem in case when there is a HUGE objectList on entrance 
        // would be nice to write into file by chunks
        string[] lines = objectList.Select(item =>
            $"{item.tpep_pickup_datetime},{item.tpep_dropoff_datetime},{item.passenger_count},{item.trip_distance},{item.store_and_fwd_flag},{item.PULocationID},{item.DOLocationID},{item.fare_amount},{item.tip_amount}"
        ).ToArray();

        await File.WriteAllLinesAsync(path, lines);

        return path;
    }
}