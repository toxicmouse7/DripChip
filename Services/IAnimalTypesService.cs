using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IAnimalTypesService
{
    public AnimalType? GetAnimalTypeInformation(uint id);
}