namespace DripChip.Models.DataTransferObjects.Accounts;

public class UserRepsonseDto
{
    public uint Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
}