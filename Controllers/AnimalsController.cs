using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using DripChip.Services;
using DripChip.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

namespace DripChip.Controllers;

[ApiController]
[Route("animals")]
public class AnimalsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;
    private readonly IFilterable<Animal, AnimalsFilterData> _animalsFilter;
    private readonly IMapper<Animal, AnimalCreationDto, AnimalUpdateDto, AnimalResponseDto> _animalsMapper;
    private readonly IValidator<AnimalCreationDto> _creationValidator;

    public AnimalsController(IRepository<Animal> animalsRepository,
        IFilterable<Animal, AnimalsFilterData> animalsFilter,
        IMapper<Animal, AnimalCreationDto, AnimalUpdateDto, AnimalResponseDto> animalsMapper,
        IValidator<AnimalCreationDto> creationValidator)
    {
        _animalsRepository = animalsRepository;
        _animalsFilter = animalsFilter;
        _animalsMapper = animalsMapper;
        _creationValidator = creationValidator;
    }

    [HttpGet]
    [Route("{animalId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetAnimalInformation(uint? animalId)
    {
        if (animalId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            return new JsonResult(_animalsMapper.ToResponse(animal));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("search")]
    [MightBeUnauthenticated]
    public IActionResult SearchAnimals([FromQuery] AnimalsFilterData animalsFilterData,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        return new JsonResult(
            _animalsFilter.Search(animalsFilterData, (int) from, (int) size)
                .Select(_animalsMapper.ToResponse)
        );
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewAnimal([FromBody] AnimalCreationDto animalCreationDto)
    {
        if (!animalCreationDto.AnimalTypes.Any() || !Enum.IsDefined(typeof(Animal.Gender), animalCreationDto.Gender))
            return BadRequest();

        try
        {
            _creationValidator.ValidateAndThrow(animalCreationDto);
            var createdAnimal = _animalsMapper.Create(animalCreationDto);
            _animalsRepository.Create(createdAnimal);
            return new JsonResult(_animalsMapper.ToResponse(createdAnimal))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{animalId?}")]
    [Authorize]
    public IActionResult UpdateAnimal(uint? animalId, [FromBody] AnimalUpdateDto animalUpdateDto)
    {
        if (animalId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var updatedAnimal = _animalsMapper.Update(animal, animalUpdateDto);
            _animalsRepository.Update(updatedAnimal);
            return new JsonResult(_animalsMapper.ToResponse(updatedAnimal));
        }
        catch (InvalidLocationChangeException e)
        {
            return BadRequest(e.Message);
        }
        catch (IncorrectLifeStatusException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{animalId?}")]
    [Authorize]
    public IActionResult DeleteAnimal(uint? animalId)
    {
        if (animalId is null or 0)
            return BadRequest();

        try
        {
            _animalsRepository.Delete(animalId.Value);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{animalId?}/types/{typeId?}")]
    [Authorize]
    public IActionResult AttachType(uint? animalId, uint? typeId, IRepository<AnimalType> typeRepository)
    {
        if (animalId is null or 0 || typeId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var type = typeRepository.Get(typeId.Value);

            if (animal.Types.Contains(type))
                return Conflict();

            animal.Types.Add(type);
            _animalsRepository.Update(animal);

            return new JsonResult(_animalsMapper.ToResponse(animal));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("/animals/{animalId?}/types")]
    [Authorize]
    public IActionResult UpdateType(uint? animalId, [FromBody] AnimalTypeUpdateDto animalTypeUpdateDto,
        IRepository<AnimalType> typeRepository)
    {
        if (animalId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var oldType = typeRepository.Get(animalTypeUpdateDto.OldTypeId);
            var newType = typeRepository.Get(animalTypeUpdateDto.NewTypeId);

            if (!animal.Types.Contains(oldType))
                throw new EntityNotFoundException();
            if (animal.Types.Contains(newType))
                return Conflict();

            animal.Types.Remove(oldType);
            animal.Types.Add(newType);
            _animalsRepository.Update(animal);

            return new JsonResult(_animalsMapper.ToResponse(animal));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{animalId?}/types/{typeId?}")]
    [Authorize]
    public IActionResult DetachType(uint? animalId, uint? typeId, IRepository<AnimalType> typeRepository)
    {
        if (animalId is null or 0 || typeId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var type = typeRepository.Get(typeId.Value);

            if (!animal.Types.Contains(type))
                throw new EntityNotFoundException();
            if (animal.Types.Count == 1)
                return BadRequest();

            animal.Types.Remove(type);
            _animalsRepository.Update(animal);

            return new JsonResult(_animalsMapper.ToResponse(animal));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}