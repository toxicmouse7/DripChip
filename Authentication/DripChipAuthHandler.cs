﻿using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using DripChip.Exceptions;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DripChip.Authentication;

public partial class DripChipAuthHandler : AuthenticationHandler<DripChipAuthSchemeOptions>
{
    private readonly IRepository<User> _accountsService;

    public DripChipAuthHandler(IOptionsMonitor<DripChipAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IRepository<User> accountsService) : base(options, logger, encoder, clock)
    {
        _accountsService = accountsService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        TokenModel model;

        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            return Task.FromResult(AuthenticateResult.Fail("Header not found"));

        var header = Request.Headers[HeaderNames.Authorization].ToString();
        var tokenMatch = TokenRegex().Match(header);

        if (!tokenMatch.Success)
            return Task.FromResult(AuthenticateResult.Fail("Token mismatch"));

        var token = tokenMatch.Groups["token"].Value;

        try
        {
            var parsedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var authData = parsedToken.Split(':');
            if (authData.Length != 2)
                return Task.FromResult(AuthenticateResult.Fail("Incorrect data"));

            var (login, password) = (authData[0], authData[1]);
            User foundUser;
            try
            {
                foundUser = _accountsService.Get(user => user.Email == login && user.Password == password);
            }
            catch (EntityNotFoundException e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e.Message));
            }

            model = new TokenModel
            {
                Id = foundUser.Id,
                Email = login,
                Password = password
            };
        }
        catch (Exception)
        {
            return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, model.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, nameof(DripChipAuthHandler));

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    [GeneratedRegex("Basic (?<token>.*)")]
    private static partial Regex TokenRegex();
}