using DripChip.Authentication;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals/types")]
public class AnimalTypesController : ControllerBase
{
    private readonly IRepository<AnimalType> _animalTypesRepository;

    public AnimalTypesController(IRepository<AnimalType> animalTypesRepository)
    {
        _animalTypesRepository = animalTypesRepository;
    }

    [HttpGet]
    [Route("{typeId}")]
    [MightBeUnauthenticated]
    public IActionResult GetTypeInformation(uint? typeId)
    {
        if (typeId is null or 0)
            return BadRequest();

        var animalType = _animalTypesRepository.Get(typeId.Value);
        if (animalType is null)
            return NotFound();

        return new JsonResult(animalType);
    }
}