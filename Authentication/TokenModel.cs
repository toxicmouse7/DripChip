namespace DripChip.Authentication;

public class TokenModel
{
    public uint Id { get; init; }
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}