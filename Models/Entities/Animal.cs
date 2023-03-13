using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DripChip.Models.Entities;

public class Animal : Entity
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum LifeStatus
    {
        Alive,
        Dead
    }

    public ICollection<AnimalType> Types { get; set; } = null!;
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Gender AnimalGender { get; set; }
    public LifeStatus AnimalLifeStatus { get; set; }
    public DateTimeOffset ChippingDateTime { get; init; }
    public User AnimalChipper { get; set; } = null!;
    public Location ChippingLocation { get; set; } = null!;
    public ICollection<VisitedLocation> VisitedLocations { get; set; }
    public DateTime? DeathDateTime { get; set; }
    
    public Animal()
    {
        VisitedLocations = new List<VisitedLocation>();
    }
}