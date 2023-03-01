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
    
    public AnimalType[] Types { get; set; } = null!;
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Gender AnimalGender { get; set; }
    public LifeStatus AnimalLifeStatus { get; set; }
    public DateTime ChippingDateTime { get; set; }
    public User AnimalChipper { get; set; } = null!;
    public Location ChippingLocation { get; set; } = null!;
    public VisitedLocation[] VisitedLocations { get; set; } = null!;
    public DateTime? DeathDateTime { get; set; }
}