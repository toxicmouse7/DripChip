using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.VisitedLocations;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals/{animalId:min(1)}/locations")]
public class AnimalLocationsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;
    private readonly IRepository<VisitedLocation> _locationsRepository;

    public AnimalLocationsController(IRepository<Animal> animalsRepository,
        IRepository<VisitedLocation> locationsRepository)
    {
        _animalsRepository = animalsRepository;
        _locationsRepository = locationsRepository;
    }

    [HttpGet]
    [MightBeUnauthenticated]
    public IActionResult GetVisitedLocations(uint animalId,
        [FromQuery] DateTime? startDateTime,
        [FromQuery] DateTime? endDateTime,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        try
        {
            var animal = _animalsRepository.Get(animalId);

            return new JsonResult(
                animal.VisitedLocations
                    .Where(visitedLocation =>
                        visitedLocation.Time >= startDateTime && visitedLocation.Time <= endDateTime)
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
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{pointId?}")]
    [Authorize]
    public IActionResult AddLocation(uint? animalId, uint? pointId)
    {
        if (animalId is null or 0 || pointId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var visitedLocation = _locationsRepository.Get(pointId.Value);

            if (animal.AnimalLifeStatus == Animal.LifeStatus.Dead)
                return BadRequest();
            if (!animal.VisitedLocations.Any() && visitedLocation.Location == animal.ChippingLocation)
                return BadRequest();
            if (animal.VisitedLocations.Contains(visitedLocation))
                return BadRequest();

            animal.VisitedLocations.Add(visitedLocation);
            _animalsRepository.Update(animal);

            return new JsonResult(new VisitedLocationsResponseDto
            {
                Id = visitedLocation.Id,
                DateTimeOfVisitLocationPoint = DateTime.Now,
                LocationPointId = visitedLocation.Location.Id
            });
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public IActionResult UpdateVisitedLocation(uint? animalId,
        [FromBody] VisitedLocationUpdateDto visitedLocationUpdateDto)
    {
        if (animalId is null or 0 ||
            visitedLocationUpdateDto.VisitedLocationPointId == visitedLocationUpdateDto.LocationPointId)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var visitedLocation = _locationsRepository.Get(visitedLocationUpdateDto.VisitedLocationPointId);
            var location = _locationsRepository.Get(visitedLocationUpdateDto.LocationPointId);

            if (!animal.VisitedLocations.Contains(visitedLocation))
                return NotFound();

            var indexOfLocation = animal.VisitedLocations
                .OrderBy(x => x.Time)
                .Select((l, i) => (l, i))
                .First(x => x.l == visitedLocation).i;

            if (animal.VisitedLocations.FirstOrDefault() == visitedLocation &&
                location.Location == animal.ChippingLocation)
                return BadRequest();
            if (animal.VisitedLocations.OrderBy(x => x.Time).ElementAtOrDefault(indexOfLocation - 1) == location
                || animal.VisitedLocations.OrderBy(x => x.Time).ElementAtOrDefault(indexOfLocation + 1) == location)
                return BadRequest();

            animal.VisitedLocations.Remove(visitedLocation);
            animal.VisitedLocations.Add(location);
            _animalsRepository.Update(animal);
            return new JsonResult(new VisitedLocationsResponseDto
            {
                Id = location.Id,
                DateTimeOfVisitLocationPoint = location.Time,
                LocationPointId = location.Location.Id
            });
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{visitedPointId?}")]
    [Authorize]
    public IActionResult RemoveVisitedLocation(uint? animalId, uint? visitedPointId)
    {
        if (animalId is null or 0 || visitedPointId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            var visitedLocation = _locationsRepository.Get(visitedPointId.Value);

            if (!animal.VisitedLocations.Contains(visitedLocation))
                return NotFound();

            animal.VisitedLocations.Remove(visitedLocation);
            if (animal.VisitedLocations.FirstOrDefault()?.Location == animal.ChippingLocation)
                animal.VisitedLocations.Remove(animal.VisitedLocations.First());

            _animalsRepository.Update(animal);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}