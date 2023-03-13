using System.ComponentModel.DataAnnotations;

namespace DripChip.Models.DataTransferObjects.Accounts;

public class UserUpdateDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}