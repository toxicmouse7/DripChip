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

    public Location? Get(uint id)
    {
        return _applicationContext.Locations.Find(id);
    }

    public Location? Get(Func<Location, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Location Update(Location entity)
    {
        throw new NotImplementedException();
    }

    public Location Create(Location entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(uint id)
    {
        throw new NotImplementedException();
    }
}