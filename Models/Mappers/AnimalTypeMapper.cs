using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.Mappers;

public class AnimalTypeMapper : IMapper<AnimalType, AnimalTypeCreationDto, AnimalTypeCreationDto, AnimalTypeResponseDto>
{
    public AnimalType Create(AnimalTypeCreationDto dto)
    {
        return new AnimalType
        {
            Type = dto.Type
        };
    }

    public AnimalType Update(AnimalType entity, AnimalTypeCreationDto dto)
    {
        entity.Type = dto.Type;
        return entity;
    }

    public AnimalTypeResponseDto ToResponse(AnimalType entity)
    {
        return new AnimalTypeResponseDto
        {
            Id = entity.Id,
            Type = entity.Type
        };
    }
}