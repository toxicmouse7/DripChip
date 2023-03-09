using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects;

public static class AnimalTypeMapper
{
    public static AnimalType FromDto(AnimalTypeDto animalTypeDto)
    {
        return new AnimalType
        {
            Type = animalTypeDto.Type
        };
    }
}