using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public class AnimalTypesService : IAnimalTypesService
{
    private readonly ApplicationContext _applicationContext;

    public AnimalTypesService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public AnimalType? GetAnimalTypeInformation(uint id)
    {
        return _applicationContext.AnimalTypes.Find(id);
    }
}