using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSaver.DB.Models;


public class Delivery
{
    [Key]
    public int id { get; set; }
    public DateTime tpep_pickup_datetime { get; set; }
    public DateTime tpep_dropoff_datetime { get; set; }
    public int passenger_count { get; set; }
    public float trip_distance { get; set; }

    // convert store_and_fwd_flag
    private string? _store_and_fwd_flag;
    public string? store_and_fwd_flag
    {
        get => _store_and_fwd_flag;
        set
        {
            if (value?.Replace(" ", "") == "N")
                _store_and_fwd_flag = "No";
            else if (value?.Replace(" ", "") == "Y")
                _store_and_fwd_flag = "Yes";
            else
                _store_and_fwd_flag = value;
        }
    }
    public int PULocationID { get; set; }
    public int DOLocationID { get; set; }
    public float fare_amount { get; set; }
    public float tip_amount { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj is not Delivery other)
            return false;
        
        return tpep_pickup_datetime == other.tpep_pickup_datetime
            && tpep_dropoff_datetime == other.tpep_dropoff_datetime
            && passenger_count == other.passenger_count;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(tpep_pickup_datetime, tpep_dropoff_datetime, passenger_count);
    }
}