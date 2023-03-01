using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _locationsService;

    public LocationsController(IRepository<Location> locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpGet]
    [Route("{pointId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetLocationInformation([Range(1, uint.MaxValue)] uint? pointId)
    {
        if (pointId is null)
            return BadRequest();

        var location = _locationsService.Get(pointId.Value);
        if (location is null)
            return NotFound();

        return new JsonResult(location);
    }
}