using System.ComponentModel.DataAnnotations;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("animals")]
public class AnimalsController : ControllerBase
{
    private readonly IRepository<Animal> _animalsRepository;
    private readonly IFilterable<Animal, AnimalsFilterData> _animalsFilter;
    private readonly IDtoMapper<Animal, AnimalCreationDto> _animalMapper;

    public AnimalsController(IRepository<Animal> animalsRepository,
        IFilterable<Animal, AnimalsFilterData> animalsFilter,
        IDtoMapper<Animal, AnimalCreationDto> animalMapper)
    {
        _animalsRepository = animalsRepository;
        _animalsFilter = animalsFilter;
        _animalMapper = animalMapper;
    }

    [HttpGet]
    [Route("{animalId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetAnimalInformation(uint? animalId)
    {
        if (animalId is null or 0)
            return BadRequest();

        try
        {
            var animal = _animalsRepository.Get(animalId.Value);
            return new JsonResult(animal);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("search")]
    [MightBeUnauthenticated]
    public IActionResult SearchAnimals([FromQuery] AnimalsFilterData animalsFilterData,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        return new JsonResult(_animalsFilter.Search(animalsFilterData, (int) from, (int) size));
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddNewAnimal([FromBody] AnimalCreationDto animalCreationDto)
    {
        if (!animalCreationDto.AnimalTypes.Any() || !Enum.IsDefined(typeof(Animal.Gender), animalCreationDto.Gender))
            return BadRequest();

        _animalMapper.FromDto<AnimalCreationDto>(animalCreationDto);

        try
        {
            var createdAnimal = _animalsRepository.Create(_animalMapper.FromDto(animalCreationDto));
            return new JsonResult(createdAnimal)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (DuplicateEntityException e)
        {
            return Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}