using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;

namespace DripChip.Services;

public class AnimalsRepository : IRepository<Animal>, IFilterable<Animal, AnimalsSearchInformation>
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

    public void Delete(Animal entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Animal> Search(AnimalsSearchInformation animalsSearchInformation, int from, int size)
    {
        var animals = _applicationContext.Animals;

        return animals
            // .Include(animal => animal.AnimalChipper)
            // .Include(animal => animal.ChippingLocation)
            .WhereIf(animalsSearchInformation.StartDateTime != null,
                animal => animal.ChippingDateTime >= animalsSearchInformation.StartDateTime)
            .WhereIf(animalsSearchInformation.EndDateTime != null,
                animal => animal.ChippingDateTime <= animalsSearchInformation.EndDateTime)
            .WhereIf(animalsSearchInformation.ChipperId != null,
                animal => animal.AnimalChipper.Id == animalsSearchInformation.ChipperId)
            .WhereIf(animalsSearchInformation.ChippingLocationId != null,
                animal => animal.ChippingLocation.Id == animalsSearchInformation.ChippingLocationId)
            .WhereIf(animalsSearchInformation.LifeStatus != null,
                animal => animal.AnimalLifeStatus == animalsSearchInformation.LifeStatus)
            .WhereIf(animalsSearchInformation.Gender != null,
                animal => animal.AnimalGender == animalsSearchInformation.Gender)
            .OrderBy(animal => animal.Id).Skip(from).Take(size);
    }
}