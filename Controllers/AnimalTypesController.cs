using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Route("{typeId?}")]
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

    [HttpPost]
    [Authorize]
    public IActionResult AddNewType([FromBody] AnimalTypeDto animalTypeDto)
    {
        try
        {
            var createdType = _animalTypesRepository.Create(AnimalTypeMapper.FromDto(animalTypeDto));
            return new JsonResult(createdType)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut("{animalTypeId?}")]
    [Authorize]
    public IActionResult UpdateAnimalType(uint? animalTypeId, [FromBody] AnimalTypeDto animalTypeDto)
    {
        if (animalTypeId is null)
            return BadRequest();
        
        var animalType = AnimalTypeMapper.FromDto(animalTypeDto);
        animalType.Id = animalTypeId.Value;
        try
        {
            var updateResult = _animalTypesRepository.Update(animalType);
            return new JsonResult(updateResult);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{animalTypeId?}")]
    [Authorize]
    public IActionResult DeleteAnimalType(uint? animalTypeId)
    {
        if (animalTypeId is null)
            return BadRequest();

        try
        {
            _animalTypesRepository.Delete(animalTypeId.Value);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}