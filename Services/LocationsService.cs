﻿using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public class LocationsService : IRepository<Location>
{
    private readonly ApplicationContext _applicationContext;

    public LocationsService(ApplicationContext applicationContext)
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

    public void Delete(Location entity)
    {
        throw new NotImplementedException();
    }
}