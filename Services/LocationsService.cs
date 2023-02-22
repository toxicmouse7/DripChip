using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public class LocationsService : ILocationsService
{
    private readonly ApplicationContext _applicationContext;

    public LocationsService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Location? GetLocationInformation(uint id)
    {
        return _applicationContext.Locations.Find(id);
    }
}