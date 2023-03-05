using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly IRepository<User> _accountsRepository;
    private readonly IFilterable<User, UsersFilterData> _accountsFilter;

    public AccountsController(IRepository<User> accountsRepository,
        IFilterable<User, UsersFilterData> accountsFilter)
    {
        _accountsRepository = accountsRepository;
        _accountsFilter = accountsFilter;
    }

    [HttpGet("{accountId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetUserInformation([Range(1, uint.MaxValue)] uint? accountId)
    {
        if (accountId is null)
            return BadRequest();

        var user = _accountsRepository.Get(accountId.Value);
        if (user is null)
            return NotFound();

        return new JsonResult(UserMapper.ToDto(user));
    }

    [HttpPut("{accountId?}")]
    [Authorize]
    public IActionResult UpdateUserInformation(uint? accountId, [FromBody] User user)
    {
        if (accountId is null)
            return BadRequest();

        #region Refactor
        
        var authenticatedId = uint.Parse(
            User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
        if (accountId != authenticatedId)
            return Forbid();

        #endregion

        user.Id = accountId.Value;

        try
        {
            var updateResult = _accountsRepository.Update(user);
            return new JsonResult(updateResult);
        }
        catch (DuplicateNameException e)
        {
            return Conflict(e.Message);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return Forbid(e.Message);
        }
    }

    [HttpGet("search")]
    [MightBeUnauthenticated]
    public IActionResult Search([FromQuery] UsersFilterData filterData,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        var foundAccounts = _accountsFilter.Search(filterData, (int) from, (int) size);
        return new JsonResult(foundAccounts.Select(UserMapper.ToDto));
    }

    [HttpPost]
    [Route("/registration")]
    public IActionResult RegisterAccount([FromBody] UserCreationDto user)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();

        try
        {
            var createdUser = _accountsRepository.Create(UserMapper.FromDto(user));
            return new JsonResult(UserMapper.ToDto(createdUser))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("{accountId?}")]
    [Authorize]
    public IActionResult DeleteAccount(uint? accountId)
    {
        if (accountId is null)
            return BadRequest();
        
        #region Refactor
        
        var authenticatedId = uint.Parse(
            User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
        if (accountId != authenticatedId)
            return Forbid();

        #endregion

        try
        {
            _accountsRepository.Delete(accountId.Value);
        }
        catch (ArgumentException e)
        {
            return Forbid(e.Message);
        }
        catch (LinkedWithAnimalException e)
        {
            return Forbid(e.Message);
        }

        return Ok();
    }
}