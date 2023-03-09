using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _locationsRepository;

    public LocationsController(IRepository<Location> locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }

    [HttpGet("{pointId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetLocationInformation([Range(1, uint.MaxValue)] uint? pointId)
    {
        if (pointId is null)
            return BadRequest();

        var location = _locationsRepository.Get(pointId.Value);
        if (location is null)
            return NotFound();

        return new JsonResult(location);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewLocation([FromBody] LocationDto locationDto)
    {
        try
        {
            var createdLocation = _locationsRepository.Create(LocationMapper.FromDto(locationDto));
            return new JsonResult(createdLocation);
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut( "{pointId?}")]
    [Authorize]
    public IActionResult UpdateLocation(uint? pointId, [FromBody] LocationDto locationDto)
    {
        if (pointId is null)
            return BadRequest();

        var entity = LocationMapper.FromDto(locationDto);
        entity.Id = pointId.Value;

        try
        {
            var result = _locationsRepository.Update(entity);
            return new JsonResult(result);
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