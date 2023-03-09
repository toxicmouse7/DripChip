using System.ComponentModel.DataAnnotations;

namespace DripChip.Models.DataTransferObjects.Locations;

public class LocationDto
{
    [Range(-90, 90)]
    public double Latitude { get; set; }
    [Range(-180, 180)]
    public double Longitude { get; set; }
}