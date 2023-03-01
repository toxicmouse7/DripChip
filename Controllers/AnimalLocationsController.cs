using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals/{animalId:min(1)}/locations")]
public class AnimalLocationsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;

    public AnimalLocationsController(IRepository<Animal> animalsRepository)
    {
        _animalsRepository = animalsRepository;
    }

    [HttpGet]
    [MightBeUnauthenticated]
    public IActionResult GetVisitedLocations(uint animalId,
        [FromQuery] DateTime? startDateTime,
        [FromQuery] DateTime? endDateTime,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        var animal = _animalsRepository.Get(animalId);
        if (animal is null)
            return NotFound();

        return new JsonResult(
            animal.VisitedLocations
                .Where(visitedLocation => visitedLocation.Time >= startDateTime && visitedLocation.Time <= endDateTime)
                .Select(visitedLocation => new
                {
                    id = animalId,
                    dateTimeOfVisitLocationPoint = visitedLocation.Time,
                    locationPointId = visitedLocation.Location.Id
                })
                .OrderBy(location => location.id)
                .Skip((int) from).Take((int) size)
        );
    }
}