using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;
using DripChip.Models.Mappers;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _locationsRepository;
    private readonly IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto> _locationsMapper;

    public LocationsController(IRepository<Location> locationsRepository,
        IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto> locationsMapper)
    {
        _locationsRepository = locationsRepository;
        _locationsMapper = locationsMapper;
    }

    [HttpGet("{pointId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetLocationInformation([Range(1, uint.MaxValue)] uint? pointId)
    {
        if (pointId is null)
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
    }

    [HttpPut( "{pointId?}")]
    [Authorize]
    public IActionResult UpdateLocation(uint? pointId, [FromBody] LocationCreationDto locationCreationDto)
    {
        if (pointId is null)
            return BadRequest();

        try
        {
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
    }

    [HttpDelete("{pointId?}")]
    [Authorize]
    public IActionResult DeleteLocation(uint? pointId)
    {
        if (pointId is null)
            return BadRequest();
        
        _locationsRepository.Delete(pointId.Value);
        return Ok();
    }
}