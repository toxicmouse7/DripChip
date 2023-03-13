using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;
using DripChip.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

namespace DripChip.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _locationsRepository;
    private readonly IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto> _locationsMapper;
    private readonly IValidator<LocationCreationDto> _creationValidator;

    public LocationsController(IRepository<Location> locationsRepository,
        IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto> locationsMapper,
        IValidator<LocationCreationDto> creationValidator)
    {
        _locationsRepository = locationsRepository;
        _locationsMapper = locationsMapper;
        _creationValidator = creationValidator;
    }

    [HttpGet("{pointId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetLocationInformation([Range(1, uint.MaxValue)] uint? pointId)
    {
        if (pointId is null or 0)
            return BadRequest();

        try
        {
            var location = _locationsRepository.Get(pointId.Value);
            return new JsonResult(_locationsMapper.ToResponse(location));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewLocation([FromBody] LocationCreationDto locationCreationDto)
    {
        try
        {
            _creationValidator.ValidateAndThrow(locationCreationDto);
            var location = _locationsMapper.Create(locationCreationDto);
            _locationsRepository.Create(location);
            return new JsonResult(_locationsMapper.ToResponse(location))
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

    [HttpPut("{pointId?}")]
    [Authorize]
    public IActionResult UpdateLocation(uint? pointId, [FromBody] LocationCreationDto locationCreationDto)
    {
        if (pointId is null or 0)
            return BadRequest();

        try
        {
            _creationValidator.ValidateAndThrow(locationCreationDto);
            var location = _locationsRepository.Get(pointId.Value);
            var updatedAnimal = _locationsMapper.Update(location, locationCreationDto);
            _locationsRepository.Update(updatedAnimal);
            return new JsonResult(_locationsMapper.ToResponse(updatedAnimal));
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

    [HttpDelete("{pointId?}")]
    [Authorize]
    public IActionResult DeleteLocation(uint? pointId)
    {
        if (pointId is null or 0)
            return BadRequest();

        try
        {
            _locationsRepository.Delete(pointId.Value);
            return Ok();
        }
        catch (LinkedWithAnimalException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}