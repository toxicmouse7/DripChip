using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.DataTransferObjects;

public sealed class AnimalMapper : IDtoMapper<Animal>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AnimalType> _animalTypesRepository;
    private readonly IRepository<Location> _locationRepository;

    public AnimalMapper(IRepository<User> userRepository, IRepository<AnimalType> animalTypesRepository,
        IRepository<Location> locationRepository)
    {
        _userRepository = userRepository;
        _animalTypesRepository = animalTypesRepository;
        _locationRepository = locationRepository;
    }

    public Animal FromDto(AnimalCreationDto creationDto)
    {
        return new Animal
        {
            AnimalChipper = _userRepository.Get(creationDto.ChipperId),
            Types = creationDto.AnimalTypes.Select(animalTypeId => _animalTypesRepository.Get(animalTypeId)).ToList(),
            ChippingLocation = _locationRepository.Get(creationDto.ChippingLocationId),
            Height = creationDto.Height,
            Weight = creationDto.Weight,
            Length = creationDto.Length,
            AnimalGender = creationDto.Gender
        };
    }

    public Animal FromDto<TDto>(TDto dto)
    {
        throw new NotImplementedException();
    }

    public TDto ToDto<TDto>(Animal entity)
    {
        throw new NotImplementedException();
    }
}