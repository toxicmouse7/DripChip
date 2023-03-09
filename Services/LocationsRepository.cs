using DripChip.Exceptions;
using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public class LocationsRepository : IRepository<Location>
{
    private readonly ApplicationContext _applicationContext;

    public LocationsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Location Get(uint id)
    {
        var foundLocation = _applicationContext.Locations.Find(id);
        if (foundLocation is null)
            throw new EntityNotFoundException();

        return foundLocation;
    }

    public Location Get(Func<Location, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Location Update(Location entity)
    {
        var locations = _applicationContext.Locations;
        if (locations.Find(entity.Id) is null)
            throw new EntityNotFoundException();

        if (locations.Any(loc => Math.Abs(loc.Latitude - entity.Latitude) < 1e-6
                                 && Math.Abs(loc.Longitude - entity.Longitude) < 1e-6))
            throw new DuplicateEntityException();

        var updatedLocation = locations.Update(entity).Entity;
        _applicationContext.SaveChanges();

        return updatedLocation;
    }

    public Location Create(Location entity)
    {
        var locations = _applicationContext.Locations;

        if (locations.Any(loc => Math.Abs(loc.Longitude - entity.Longitude) < 1e-6 &&
                                 Math.Abs(loc.Latitude - entity.Latitude) < 1e-6))
            throw new DuplicateEntityException();

        return _applicationContext.Locations.Add(entity).Entity;
    }

    public void Delete(uint id)
    {
        var locations = _applicationContext.Locations;
        var foundLocation = locations.Find(id);
        if (foundLocation is null)
            throw new EntityNotFoundException();
        
        locations.Remove(foundLocation);
        _applicationContext.SaveChanges();
    }
}