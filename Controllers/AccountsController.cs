using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DripChip.Authentication;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;
using DripChip.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [HttpGet("{accountId?}")]
    [MightBeUnauthenticated]
    public IActionResult GetUserInformation([Range(1, uint.MaxValue)] uint? accountId)
    {
        if (accountId is null)
            return BadRequest();

        var user = _accountsService.GetUserInformation(accountId.Value);
        if (user is null)
            return NotFound();

        return new JsonResult(new UserDataTransfer(user));
    }

    [HttpPut("{accountId?}")]
    // [Authorize]
    public IActionResult UpdateUserInformation(uint? accountId, [FromBody] User user)
    {
        if (accountId is null)
            return BadRequest();

        var authenticatedId = uint.Parse(User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

        if (accountId != authenticatedId)
            return Forbid();
        
        return Ok();
    }

    [HttpGet("search")]
    [MightBeUnauthenticated]
    public IActionResult Search([FromQuery] UsersSearchInformation searchInformation,
        [FromQuery] uint from = 0,
        [FromQuery] [Range(1, uint.MaxValue)]
        uint size = 10)
    {
        var foundAccounts = _accountsService.SearchUsers(searchInformation, (int) from, (int) size);
        return new JsonResult(foundAccounts.Select(user => new UserDataTransfer(user)));
    }

    [HttpPost]
    [Route("/registration")]
    public IActionResult RegisterAccount([FromBody] User user)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();

        try
        {
            var createdUser = _accountsService.CreateNew(user);
            return new JsonResult(new UserDataTransfer(createdUser))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
    }
}