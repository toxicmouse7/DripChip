using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;

namespace DripChip.Services;

public interface IAnimalsService
{
    public Animal? GetAnimalInformation(uint id);
    public Animal[] SearchAnimals(AnimalsSearchInformation animalsSearchInformation, int from, int size);
}