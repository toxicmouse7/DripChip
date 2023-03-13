using DripChip.Exceptions;
using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Services;

public class AnimalsRepository : IRepository<Animal>, IFilterable<Animal, AnimalsFilterData>
{
    private readonly ApplicationContext _applicationContext;

    public AnimalsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Animal Get(uint id)
    {
        var foundAnimal = _applicationContext.Animals
            .Include(x => x.ChippingLocation)
            .Include(x => x.Types)
            .FirstOrDefault(x => x.Id == id);
        if (foundAnimal is null)
            throw new EntityNotFoundException();

        return foundAnimal;
    }

    public Animal Get(Func<Animal, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public void Update(Animal entity)
    {
        _applicationContext.Update(entity);
        _applicationContext.SaveChanges();
    }

    public void Create(Animal entity)
    {
        if (entity.Types.Count != entity.Types.Distinct().Count())
            throw new DuplicateEntityException();
        _applicationContext.Animals.Add(entity);
        _applicationContext.SaveChanges();
    }

    public void Delete(uint id)
    {
        var animals = _applicationContext.Animals;
        var animal = animals.Find(id);
        if (animal is null)
            throw new EntityNotFoundException();

        if (!animal.VisitedLocations.Any())
            throw new InvalidOperationException();

        animals.Remove(animal);
        _applicationContext.SaveChanges();
    }

    public IEnumerable<Animal> Search(AnimalsFilterData animalsFilterData, int from, int size)
    {
        var animals = _applicationContext.Animals;

        return animals
            .WhereIf(animalsFilterData.StartDateTime != null,
                animal => animal.ChippingDateTime >= animalsFilterData.StartDateTime)
            .WhereIf(animalsFilterData.EndDateTime != null,
                animal => animal.ChippingDateTime <= animalsFilterData.EndDateTime)
            .WhereIf(animalsFilterData.ChipperId != null,
                animal => animal.AnimalChipper.Id == animalsFilterData.ChipperId)
            .WhereIf(animalsFilterData.ChippingLocationId != null,
                animal => animal.ChippingLocation.Id == animalsFilterData.ChippingLocationId)
            .WhereIf(animalsFilterData.LifeStatus != null,
                animal => animal.AnimalLifeStatus == animalsFilterData.LifeStatus)
            .WhereIf(animalsFilterData.Gender != null,
                animal => animal.AnimalGender == animalsFilterData.Gender)
            .OrderBy(animal => animal.Id).Skip(from).Take(size);
    }
}