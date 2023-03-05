using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;

namespace DripChip.Services;

public class AnimalsRepository : IRepository<Animal>, IFilterable<Animal, AnimalsFilterData>
{
    private readonly ApplicationContext _applicationContext;

    public AnimalsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Animal? Get(uint id)
    {
        return _applicationContext.Animals.Find(id);
    }

    public Animal? Get(Func<Animal, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Animal Update(Animal entity)
    {
        throw new NotImplementedException();
    }

    public Animal Create(Animal entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(uint id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Animal> Search(AnimalsFilterData animalsFilterData, int from, int size)
    {
        var animals = _applicationContext.Animals;

        return animals
            // .Include(animal => animal.AnimalChipper)
            // .Include(animal => animal.ChippingLocation)
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