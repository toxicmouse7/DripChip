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
[Route("animals/{animalId?}/locations")]
public class AnimalLocationsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;
    private readonly IRepository<Location> _locationsRepository;

    public AnimalLocationsController(IRepository<Animal> animalsRepository,
        IRepository<Location> locationsRepository)
    {
        _animalsRepository = animalsRepository;
        _locationsRepository = locationsRepository;
    }

    [HttpGet]
    [MightBeUnauthenticated]
    public IActionResult GetVisitedLocations(uint? animalId,
        [FromQuery] DateTimeOffset? startDateTime,
        [FromQuery] DateTimeOffset? endDateTime,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        if (animalId is null or 0)
            return BadRequest();
        
        try
        {
            var animal = _animalsRepository.Get(animalId.Value);

            return new JsonResult(
                animal.VisitedLocations
                    .WhereIf(startDateTime.HasValue, x => x.Time >= startDateTime)
                    .WhereIf(endDateTime.HasValue, x => x.Time <= endDateTime)
                    .Select(visitedLocation => new
                    {
                        id = visitedLocation.Id,
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
            var location = _locationsRepository.Get(pointId.Value);

            if (animal.AnimalLifeStatus == Animal.LifeStatus.Dead)
                return BadRequest();
            if (!animal.VisitedLocations.Any() && location == animal.ChippingLocation)
                return BadRequest();
            if (animal.VisitedLocations.LastOrDefault()?.Location == location)
                return BadRequest();

            var visitedLocation = new VisitedLocation
            {
                Location = location,
                Time = DateTime.Now
            };

            animal.VisitedLocations.Add(visitedLocation);
            _animalsRepository.Update(animal);

            return new JsonResult(new VisitedLocationsResponseDto
            {
                Id = visitedLocation.Id,
                DateTimeOfVisitLocationPoint = visitedLocation.Time,
                LocationPointId = visitedLocation.Location.Id
            })
            {
                StatusCode = StatusCodes.Status201Created
            };
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
            var visitedLocation = animal.VisitedLocations
                .Last(x => x.Id == visitedLocationUpdateDto.LocationPointId);
            var location = _locationsRepository.Get(visitedLocationUpdateDto.LocationPointId);

            if (!animal.VisitedLocations.Contains(visitedLocation))
                return NotFound();

            var indexOfLocation = animal.VisitedLocations
                .OrderBy(x => x.Time)
                .Select((l, i) => (l, i))
                .First(x => x.l == visitedLocation).i;

            if (animal.VisitedLocations.FirstOrDefault() == visitedLocation &&
                location == animal.ChippingLocation)
                return BadRequest();
            // if (animal.VisitedLocations.OrderBy(x => x.Time).ElementAtOrDefault(indexOfLocation - 1) == location
            //     || animal.VisitedLocations.OrderBy(x => x.Time).ElementAtOrDefault(indexOfLocation + 1) == location)
            //     return BadRequest();

            var newVisitedLocation = new VisitedLocation
            {
                Location = location,
                Time = DateTime.Now
            };

            animal.VisitedLocations.Remove(visitedLocation);
            animal.VisitedLocations.Add(newVisitedLocation);
            _animalsRepository.Update(animal);
            return new JsonResult(new VisitedLocationsResponseDto
            {
                Id =animal.VisitedLocations.Last().Id,
                DateTimeOfVisitLocationPoint = newVisitedLocation.Time,
                LocationPointId = location.Id
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

            // if (!animal.VisitedLocations.Contains(visitedLocation))
            //     return NotFound();
            //
            // animal.VisitedLocations.Remove(visitedLocation);
            // if (animal.VisitedLocations.FirstOrDefault()?.Location == animal.ChippingLocation)
            //     animal.VisitedLocations.Remove(animal.VisitedLocations.First());

            _animalsRepository.Update(animal);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}