using DripChip.Authentication;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals/types")]
public class AnimalTypesController : ControllerBase
{
    private readonly IAnimalTypesService _animalTypesService;

    public AnimalTypesController(IAnimalTypesService animalTypesService)
    {
        _animalTypesService = animalTypesService;
    }

    [HttpGet]
    [Route("{typeId}")]
    [MightBeUnauthenticated]
    public IActionResult GetTypeInformation(uint? typeId)
    {
        if (typeId is null or 0)
            return BadRequest();

        var animalType = _animalTypesService.GetAnimalTypeInformation(typeId.Value);
        if (animalType is null)
            return NotFound();

        return new JsonResult(animalType);
    }
}