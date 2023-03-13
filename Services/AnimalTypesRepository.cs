using DripChip.Exceptions;
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

    public AnimalType Get(uint id)
    {
        var foundAnimalType = _applicationContext.AnimalTypes.Find(id);
        if (foundAnimalType is null)
            throw new EntityNotFoundException();

        return foundAnimalType;
    }

    public AnimalType Get(Func<AnimalType, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public void Update(AnimalType entity)
    {
        var animalTypes = _applicationContext.AnimalTypes;
        if (animalTypes.Find(entity.Id) is null)
            throw new EntityNotFoundException();

        animalTypes.Update(entity);
    }

    public void Create(AnimalType entity)
    {
        var animalTypes = _applicationContext.AnimalTypes;
        if (animalTypes.Any(animalType => animalType.Type == entity.Type))
            throw new DuplicateEntityException();

        animalTypes.Add(entity);
        _applicationContext.SaveChanges();
    }

    public void Delete(uint id)
    {
        var animalTypes = _applicationContext.AnimalTypes;

        var foundAnimal = animalTypes.Find(id);
        if (foundAnimal is null)
            throw new EntityNotFoundException();

        animalTypes.Remove(foundAnimal);
        _applicationContext.SaveChanges();
    }
}