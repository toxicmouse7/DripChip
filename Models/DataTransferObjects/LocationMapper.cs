using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects;

public static class LocationMapper
{
    public static Location FromDto(LocationDto locationDto)
    {
        return new Location
        {
            Longitude = locationDto.Longitude,
            Latitude = locationDto.Latitude
        };
    }
}