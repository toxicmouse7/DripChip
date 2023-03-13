using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.Mappers;

public sealed class AnimalMapper : IMapper<Animal, AnimalCreationDto, AnimalUpdateDto, AnimalResponseDto>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<AnimalType> _animalTypeRepository;

    public AnimalMapper(IRepository<User> userRepository,
        IRepository<Location> locationRepository, IRepository<AnimalType> animalTypeRepository)
    {
        _userRepository = userRepository;
        _locationRepository = locationRepository;
        _animalTypeRepository = animalTypeRepository;
    }

    public Animal Create(AnimalCreationDto dto)
    {
        return new Animal
        {
            Types = dto.AnimalTypes.Select(x => _animalTypeRepository.Get(x)).ToList(),
            Weight = dto.Weight,
            Length = dto.Length,
            Height = dto.Height,
            AnimalGender = dto.Gender,
            AnimalLifeStatus = Animal.LifeStatus.Alive,
            ChippingDateTime = DateTime.Now,
            AnimalChipper = _userRepository.Get(dto.ChipperId),
            ChippingLocation = _locationRepository.Get(dto.ChippingLocationId),
            DeathDateTime = null
        };
    }

    public Animal Update(Animal entity, AnimalUpdateDto dto)
    {
        if (entity.AnimalLifeStatus == Animal.LifeStatus.Dead && dto.LifeStatus == Animal.LifeStatus.Alive)
            throw new IncorrectLifeStatusException();

        if (dto.ChippingLocationId == entity.VisitedLocations.FirstOrDefault()?.Id)
            throw new InvalidLocationChangeException();

        var newChipper = _userRepository.Get(dto.ChipperId);
        var newChippingLocation = _locationRepository.Get(dto.ChippingLocationId);

        entity.AnimalGender = dto.Gender;
        entity.AnimalChipper = newChipper;
        entity.ChippingLocation = newChippingLocation;
        entity.Weight = dto.Weight;
        entity.Height = dto.Height;
        entity.Length = dto.Length;
        entity.AnimalLifeStatus = dto.LifeStatus;

        return entity;
    }

    public AnimalResponseDto ToResponse(Animal entity)
    {
        return new AnimalResponseDto
        {
            Id = entity.Id,
            AnimalTypes = entity.Types.Select(x => x.Id),
            Weight = entity.Weight,
            Length = entity.Length,
            Height = entity.Height,
            Gender = entity.AnimalGender,
            LifeStatus = entity.AnimalLifeStatus,
            ChippingDateTime = entity.ChippingDateTime,
            ChipperId = entity.AnimalChipper.Id,
            ChippingLocationId = entity.ChippingLocation.Id,
            VisitedLocations = entity.VisitedLocations.Select(x => x.Id),
            DeathDateTime = entity.DeathDateTime
        };
    }
}