using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Models;
using DripChip.Models.SearchInformation;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalsService _animalsService;

    public AnimalsController(IAnimalsService animalsService)
    {
        _animalsService = animalsService;
    }

    [HttpGet]
    [Route("{animalId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetAnimalInformation(uint? animalId)
    {
        if (animalId is null or 0)
            return BadRequest();

        var animal = _animalsService.GetAnimalInformation(animalId.Value);
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
        return new JsonResult(_animalsService.SearchAnimals(animalsSearchInformation, (int) from, (int) size));
    }
}