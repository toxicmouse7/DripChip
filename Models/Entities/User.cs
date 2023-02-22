using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.Models.Entities;

public class User
{
    public uint Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}