using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
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

        var location = _locationsService.GetLocationInformation(pointId.Value);
        if (location is null)
            return NotFound();

        return new JsonResult(location);
    }
}