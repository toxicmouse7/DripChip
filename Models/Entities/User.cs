using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DripChip.Models.Entities;

public class User : Entity
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    
    public IEnumerable<Animal> ChippedAnimals { get; set; } = null!;
}