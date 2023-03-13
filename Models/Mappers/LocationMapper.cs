using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.Mappers;

public class LocationMapper : IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto>
{
    public Location Create(LocationCreationDto dto)
    {
        return new Location
        {
            Longitude = dto.Longitude,
            Latitude = dto.Latitude
        };
    }

    public Location Update(Location entity, LocationCreationDto dto)
    {
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        return entity;
    }

    public LocationResponseDto ToResponse(Location entity)
    {
        return new LocationResponseDto
        {
            Id = entity.Id,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude
        };
    }
}