namespace DripChip.Authentication;

public class TokenModel
{
    public uint Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}