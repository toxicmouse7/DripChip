using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public class AnimalTypesRepository : IRepository<AnimalType>
{
    private readonly ApplicationContext _applicationContext;

    public AnimalTypesRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public AnimalType? Get(uint id)
    {
        return _applicationContext.AnimalTypes.Find(id);
    }

    public AnimalType? Get(Func<AnimalType, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public AnimalType Update(AnimalType entity)
    {
        throw new NotImplementedException();
    }

    public AnimalType Create(AnimalType entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(AnimalType entity)
    {
        throw new NotImplementedException();
    }
}