using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using DripChip.Authentication;
using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.DataTransferObjects.Accounts;
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
    private readonly IMapper<User, UserCreationDto, UserUpdateDto, UserRepsonseDto> _userMapper;

    public AccountsController(IRepository<User> accountsRepository,
        IFilterable<User, UsersFilterData> accountsFilter,
        IMapper<User, UserCreationDto, UserUpdateDto, UserRepsonseDto> userMapper)
    {
        _accountsRepository = accountsRepository;
        _accountsFilter = accountsFilter;
        _userMapper = userMapper;
    }

    [HttpGet("{accountId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetUserInformation([Range(1, uint.MaxValue)] uint? accountId)
    // public IActionResult GetUserInformation(EntityIdDto accountId)
    {
        if (accountId is null)
            return BadRequest();

        try
        {
            var user = _accountsRepository.Get(accountId.Value);
            return new JsonResult(_userMapper.ToResponse(user));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }

    }

    [HttpPut("{accountId?}")]
    [Authorize]
    public IActionResult UpdateUserInformation(uint? accountId, [FromBody] UserUpdateDto userUpdateDto)
    {
        if (accountId is null or 0)
            return BadRequest();

        #region Refactor
        
        var authenticatedId = uint.Parse(
            User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
        if (accountId != authenticatedId)
            return Forbid();

        #endregion

        try
        {
            var user = _accountsRepository.Get(accountId.Value);
            var updatedUser = _userMapper.Update(user, userUpdateDto);
            _accountsRepository.Update(updatedUser);
            return new JsonResult(_userMapper.ToResponse(updatedUser));
        }
        catch (DuplicateNameException e)
        {
            return Conflict(e.Message);
        }
        catch (EntityNotFoundException)
        {
            return Forbid();
        }
    }

    [HttpGet("search")]
    [MightBeUnauthenticated]
    public IActionResult Search([FromQuery] UsersFilterData filterData,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)] uint size = 10)
    {
        var foundAccounts = _accountsFilter.Search(filterData, (int) from, (int) size);
        return new JsonResult(foundAccounts.Select(_userMapper.ToResponse));
    }

    [HttpPost]
    [Route("/registration")]
    public IActionResult RegisterAccount([FromBody] UserCreationDto userCreationDto)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();

        try
        {
            var user = _userMapper.Create(userCreationDto);
            _accountsRepository.Create(user);
            return new JsonResult(_userMapper.ToResponse(user))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("{accountId?}")]
    [Authorize]
    public IActionResult DeleteAccount(uint? accountId)
    {
        if (accountId is null or 0)
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
        catch (ArgumentException)
        {
            return Forbid();
        }
        catch (LinkedWithAnimalException)
        {
            return BadRequest();
        }

        return Ok();
    }
}