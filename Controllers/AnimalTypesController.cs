using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.Entities;
using DripChip.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals/types")]
public class AnimalTypesController : ControllerBase
{
    private readonly IRepository<AnimalType> _animalTypesRepository;
    private readonly IValidator<AnimalTypeCreationDto> _creationValidator;

    private readonly IMapper<AnimalType, AnimalTypeCreationDto, AnimalTypeCreationDto, AnimalTypeResponseDto>
        _typesMapper;

    public AnimalTypesController(IRepository<AnimalType> animalTypesRepository,
        IMapper<AnimalType, AnimalTypeCreationDto, AnimalTypeCreationDto, AnimalTypeResponseDto> typesMapper,
        IValidator<AnimalTypeCreationDto> creationValidator)
    {
        _animalTypesRepository = animalTypesRepository;
        _typesMapper = typesMapper;
        _creationValidator = creationValidator;
    }

    [HttpGet]
    [Route("{typeId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetTypeInformation(uint? typeId)
    {
        if (typeId is null or 0)
            return BadRequest();

        try
        {
            var animalType = _animalTypesRepository.Get(typeId.Value);
            return new JsonResult(_typesMapper.ToResponse(animalType));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewType([FromBody] AnimalTypeCreationDto animalTypeCreationDto)
    {
        try
        {
            _creationValidator.ValidateAndThrow(animalTypeCreationDto);
            var type = _typesMapper.Create(animalTypeCreationDto);
            _animalTypesRepository.Create(type);
            return new JsonResult(_typesMapper.ToResponse(type))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{animalTypeId?}")]
    [Authorize]
    public IActionResult UpdateAnimalType(uint? animalTypeId, [FromBody] AnimalTypeCreationDto animalTypeCreationDto)
    {
        if (animalTypeId is null or 0)
            return BadRequest();
        
        try
        {
            _creationValidator.ValidateAndThrow(animalTypeCreationDto);
            var type = _animalTypesRepository.Get(animalTypeId.Value);
            var updatedType = _typesMapper.Update(type, animalTypeCreationDto);
            _animalTypesRepository.Update(updatedType);
            return new JsonResult(_typesMapper.ToResponse(updatedType));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("{animalTypeId?}")]
    [Authorize]
    public IActionResult DeleteAnimalType(uint? animalTypeId)
    {
        if (animalTypeId is null or 0)
            return BadRequest();

        try
        {
            _animalTypesRepository.Delete(animalTypeId.Value);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}