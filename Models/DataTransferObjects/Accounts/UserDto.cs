﻿namespace DripChip.Models.DataTransferObjects.Accounts;

public class UserDto
{
    public uint Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
}