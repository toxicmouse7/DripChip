using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects.Animals;

public class AnimalResponseDto
{
    public uint Id { get; set; }
    public IEnumerable<uint> AnimalTypes { get; init; } = null!;
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Animal.Gender Gender { get; set; }
    public Animal.LifeStatus LifeStatus { get; set; }
    public DateTimeOffset ChippingDateTime { get; set; }
    public uint ChipperId { get; set; }
    public uint ChippingLocationId { get; set; }
    public IEnumerable<uint> VisitedLocations { get; init; } = null!;
    public DateTime? DeathDateTime { get; set; }
}