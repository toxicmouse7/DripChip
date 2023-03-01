using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;
using DripChip.Services;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals")]
public class AnimalsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;
    private readonly IFilterable<Animal, AnimalsSearchInformation> _animalsFilter;

    public AnimalsController(IRepository<Animal> animalsRepository,
        IFilterable<Animal, AnimalsSearchInformation> animalsFilter)
    {
        _animalsRepository = animalsRepository;
        _animalsFilter = animalsFilter;
    }

    [HttpGet]
    [Route("{animalId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetAnimalInformation(uint? animalId)
    {
        if (animalId is null or 0)
            return BadRequest();

        var animal = _animalsRepository.Get(animalId.Value);
        if (animal is null)
            return NotFound();

        return new JsonResult(animal);
    }

    [HttpGet]
    [Route("search")]
    [MightBeUnauthenticated]
    public IActionResult SearchAnimals([FromQuery] AnimalsSearchInformation animalsSearchInformation,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        return new JsonResult(_animalsFilter.Search(animalsSearchInformation, (int) from, (int) size));
    }
}